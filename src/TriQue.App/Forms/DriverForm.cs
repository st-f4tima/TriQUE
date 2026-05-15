using TriQue.DTOs;
using TriQue.Enums;
using TriQue.Helpers;
using TriQue.Helpers.Animation;
using TriQue.Models;
using TriQue.Services;

namespace TriQue.Forms
{
    public partial class DriverForm : Form
    {
        private readonly DriverDashboardService _dashboardService = new();
        private readonly RouteService _routeService = new();
        private readonly QueueService _queueService = new();
        private readonly TripService _tripService = new();

        private DriverDashboardDto? _dashboardData;
        private Driver? _driver;
        private Route? _route;

        private readonly int _userID;

        private bool _mapLoaded;
        private bool _goalMessageShown;

        public DriverForm(int userID)
        {
            InitializeComponent();
            DataGridTripHistory.ReadOnly = true;

            ApplyFonts();

            _userID = userID;

            LoadDashboard();
        }

        private void LoadDashboard()
        {
            _dashboardData = _dashboardService.GetDashboard(_userID);

            if (_dashboardData == null)
            {
                return;
            }

            _driver = _dashboardData.Driver;

            _route = _dashboardService.GetDriverRouteByDriverID(_driver.DriverID);

            DisplayDashboardData();
            LoadTripHistory();
            UpdateJoinButtonState();
        }

        private void DisplayDashboardData()
        {
            if (_dashboardData == null)
            {
                return;
            }

            lblGreeting.Text = $"{GetTimeBasedGreeting()}, {_dashboardData.User.FirstName}!";
            lblTodayEarningValue.Text = _dashboardData.ActualEarnings.ToString("₱ #,##0.00");
            lblEarningsGoal.Text = $"Goal: {_dashboardData.Driver.GoalEarnings:₱ 0}";
            lblTotalTripsValue.Text = _dashboardData.CompletedTrips.ToString();
            lblTripsTodayValue.Text = _dashboardData.TodayTrips.ToString();
            lblFastestTripValue.Text = $"{_dashboardData.FastestTrip:0} min";
            lblLowestTripValue.Text = $"{_dashboardData.SlowestTrip:0} min";
            lblRouteStatus.Text = $"On Route - {_dashboardData.RouteName}";
            lblTotalDistanceValue.Text = $"{_dashboardData.TotalDistance} km";

            UpdateProgressBar();
        }

        private string GetTimeBasedGreeting()
        {
            int hour = DateTime.Now.Hour;

            if (hour >= 5 && hour < 12)
            {
                return "Good Morning";
            }
            else if (hour >= 12 && hour < 18)
            {
                return "Good Afternoon";
            }
            else
            {
                // covers 6:00 PM to 4:59 AM
                return "Good Evening";
            }
        }

        private void UpdateProgressBar()
        {
            if (_dashboardData == null)
            {
                return;
            }
            int goal = (int)_dashboardData.Driver.GoalEarnings;
            int actual = (int)_dashboardData.ActualEarnings;
            int percent = goal > 0 ? (int)Math.Min((double)actual / goal * 100, 100) : 0;

            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = 100;
            ProgressBar.Value = percent;

            UpdateProgressBarColor(percent);
        }

        private void UpdateProgressBarColor(int percent)
        {
            if (percent >= 70)
            {
                ProgressBar.ProgressColorA = Color.FromArgb(34, 139, 34);
                ProgressBar.ProgressColorB = Color.FromArgb(0, 200, 0);

                if (percent >= 100)
                {
                    ShowGoalReachedMessage();
                }
            }
            else if (percent >= 30)
            {
                ProgressBar.ProgressColorA = Color.FromArgb(255, 140, 0);
                ProgressBar.ProgressColorB = Color.FromArgb(255, 200, 0);
                _goalMessageShown = false;
            }
            else
            {
                ProgressBar.ProgressColorA = Color.FromArgb(197, 34, 34);
                ProgressBar.ProgressColorB = Color.FromArgb(255, 80, 80);
                _goalMessageShown = false;
            }
        }

        private void ShowGoalReachedMessage()
        {
            if (_goalMessageShown || _dashboardData == null)
            {
                return;
            }

            _goalMessageShown = true;

            MessageBox.Show(
                $"🎉 Congratulations, {_dashboardData.User.FirstName}!\nYou've reached your daily earnings goal!",
                "Goal Reached!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void LoadTripHistory()
        {
            if (_driver == null)
                return;

            DataGridTripHistory.DataSource = _tripService.GetTripHistory(_driver.DriverID);
            DataGridTripHistory.DataBindingComplete += (s, e) =>
            {
                if (DataGridTripHistory.Columns.Count >= 3)
                {
                    DataGridTripHistory.Columns[0].Width = 120;
                    DataGridTripHistory.Columns[1].Width = 70;
                    DataGridTripHistory.Columns[2].Width = 150;
                }
            };
        }

        // automatically runs when the form opens for the first time
        private async void Form1_Load(object sender, EventArgs e)
        {
            await webView21.EnsureCoreWebView2Async();

            string key = Environment.GetEnvironmentVariable("TOMTOM_API_KEY");

            await webView21.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(
                $"window.tomtomKey = '{key}';"
            );

            webView21.CoreWebView2.WebMessageReceived += async (s, args) =>
            {
                if (args.TryGetWebMessageAsString() == "mapReady")
                {
                    _mapLoaded = true;
                    await LoadRouteToMap();
                }
            };

            string path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Assets", "map.html"
            );

            webView21.Source = new Uri(path);
        }


        // handles map view
        private async Task LoadRouteToMap()
        {
            if (!_mapLoaded || _route == null)
            {
                return;
            }

            var result = await _routeService.GetTrafficAndDuration(
                _route.StartLat, _route.StartLng,
                _route.EndLat, _route.EndLng
            );

            var payload = new object[]
            {
                new[] { _route.StartLng, _route.StartLat },
                new[] { _route.EndLng, _route.EndLat   },
                _route.RouteName
            };

            string json = System.Text.Json.JsonSerializer.Serialize(payload);

            await webView21.CoreWebView2.ExecuteScriptAsync("clearRoute();");
            await webView21.CoreWebView2.ExecuteScriptAsync($"drawRoute({json});");

            lblTrafficStatus.Text = $"{result.trafficCondition}";
            lblTrafficStatus.ForeColor = result.trafficCondition switch
            {
                "Light" => Color.FromArgb(0, 200, 83),
                "Moderate" => Color.Orange,
                "Heavy" => Color.Red,
                _ => Color.Gray
            };

            lblTotalDurationValue.Text = $"{result.durationMin} min";
        }

        private void btnJoinQueue_Click(object sender, EventArgs e)
        {
            if (_driver == null || _route == null)
            {
                return;
            }

            if (_driver.Status == DriverStatus.Finished)
            {
                _dashboardService.ResetDriverToWaiting(_driver.DriverID);
            }

            var message = _queueService.JoinQueue(_driver.DriverID, _route.RouteID);
            MessageBox.Show(message,
                    "Queue Update",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

            LoadDashboard();
            UpdateJoinButtonState();
            RefreshQueueForms();
        }

        private void UpdateJoinButtonState()
        {
            if (_driver == null || _route == null)
            {
                return;
            }

            bool alreadyInQueue = _queueService.IsDriverInQueue(_driver.DriverID, _route.RouteID);
            bool canJoin = !alreadyInQueue && (_driver.Status == DriverStatus.Waiting ||
                            _driver.Status == DriverStatus.Finished);

            btnJoinQueue.Enabled = canJoin;
            btnJoinQueue.Text = canJoin ? "Join Queue" : "Unavailable";
            btnJoinQueue.FillColor = canJoin
                ? Color.FromArgb(55, 91, 231)
                : Color.Gray;
        }

        public void RefreshJoinButton()
        {
            UpdateJoinButtonState();
        }

        private void RefreshQueueForms()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is DriverViewQueue queueForm)
                {
                    queueForm.UpdateStartButtonState();
                }
            }
        }

        #region additional styles
        private void ApplyFonts()
        {
            this.Font = FontHelper.RobotoRegular;
            lblGreeting.Font = FontHelper.GetRoboto(11f, FontStyle.Bold);
            lblGreeting.ForeColor = Color.DimGray;

            lblDesc.Text = "Ready to get started?";
            lblDesc.Font = FontHelper.GetRoboto(16f, FontStyle.Bold);

            lblTodayEarningsTitle.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblTodayEarningValue.Font = FontHelper.GetRoboto(19f, FontStyle.Bold);
            lblEarningsGoal.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);

            lblTripsTodayLabel.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblTripsTodayValue.Font = FontHelper.GetRoboto(16f, FontStyle.Bold);

            lblTotalTripsLabel.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblTotalTripsValue.Font = FontHelper.GetRoboto(16f, FontStyle.Bold);

            lblFastestTripValue.Font = FontHelper.GetRoboto(17f, FontStyle.Bold);
            lblLowestTripValue.Font = FontHelper.GetRoboto(17f, FontStyle.Bold);
            lblTotalDistanceValue.Font = FontHelper.GetRoboto(17f, FontStyle.Bold);

            lblRouteStatus.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);

            lblTripHistoryTitle.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            DataGridTripHistory.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);

            lblTrafficStatus.Font = FontHelper.GetRoboto(16f, FontStyle.Bold);
            lblTotalDurationValue.Font = FontHelper.GetRoboto(16f, FontStyle.Bold);
            lblTotalDistanceValue.Font = FontHelper.GetRoboto(16f, FontStyle.Bold);

            lblTrafficLabel.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblTotalDurationLabel.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblTotalDistanceLabel.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);

            DataGridTripHistory.DefaultCellStyle.Font = FontHelper.GetRoboto(8f, FontStyle.Bold);
            btnJoinQueue.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
        }

        #endregion

        #region navigation

        private async void ViewQueueBtn_Click(object sender, EventArgs e)
        {
            var viewQueue = new DriverViewQueue(_route.RouteID, _userID);
            await FormAnimator.SwitchAsync(this, viewQueue);
        }

        private async void DriverSettingsBtn_Click(object sender, EventArgs e)
        {
            var settings = new DriverSettings(_userID);
            await FormAnimator.SwitchAsync(this, settings);
        }

        private async void LogoutBtn_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            new AuthenticationService().Logout(_userID);

            await FormAnimator.SwitchAsync(this, new LoginForm());
        }

        #endregion

    }
}