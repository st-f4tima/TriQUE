using Microsoft.VisualBasic.Logging;
using TriQue.Data.Repositories;
using TriQue.DTOs;
using TriQue.Enums;
using TriQue.Helpers.Animation;
using TriQue.Services;

namespace TriQue.Forms
{
    public partial class DriverForm : Form
    {
        private DriverDashboardService _dashboardService;
        private RouteService _routeService;
        private QueueService _queueService;
        private DriverDashboardDto _data;
        private QueueRepository _queueRepo;
        private TripRepository _tripRepo;

        private int _userID;
        private int _routeId;
        private bool _mapLoaded = false;
        private bool _goalMessageShown = false;

        public DriverForm(int userID)
        {
            InitializeComponent();
            InitializeContext();

            _userID = userID;

            LoadDashboard();
        }

        private void InitializeContext()
        {
            _dashboardService = new DriverDashboardService();
            _queueRepo = new QueueRepository();
            _routeService = new RouteService();
            _queueService = new QueueService();
            _tripRepo = new TripRepository();
        }

        private void LoadDashboard()
        {
            _data = _dashboardService.GetDashboard(_userID);
            DisplayData();

            var driver = _dashboardService.GetDriver(_userID);
            if (driver != null)
            {
                var route = _dashboardService.GetDriverRouteByDriverID(driver.DriverID);
                if (route != null)
                {
                    _routeId = route.RouteID;
                    UpdateJoinButtonState(driver.DriverID, route.RouteID);
                }
            }
        }

        private void DisplayData()
        {
            if (_data == null || _data.User == null || _data.Driver == null)
                return;

            lblWelcomeMessage.Text = $"Welcome Back, {_data.User.FirstName}!";
            lblTodayEarningValue.Text = _data.ActualEarnings.ToString("₱ #,##0.00");
            lblEarningsGoal.Text = $"Goal: {_data.Driver.GoalEarnings.ToString("₱ 0")}";

            // progress bar
            UpdateProgressBar();

            // stats
            lblTotalTripsValue.Text = _data.CompletedTrips.ToString();
            lblTripsTodayValue.Text = _data.TodayTrips.ToString();
            lblFastestTripValue.Text = $"{_data.FastestTrip:0} min";
            lblLowestTripValue.Text = $"{_data.SlowestTrip:0} min";

            var driver = _data.Driver.DriverID;

            lblRouteStatus.Text = $"On Route - {_data.RouteName}";
            lblTotalDistanceValue.Text = $"{_data.TotalDistance} km";
            LoadDataGrid();

        }

        private void UpdateProgressBar()
        {
            int goal = (int)_data.Driver.GoalEarnings;
            int actual = (int)_data.ActualEarnings;
            int percent = goal > 0 ? (int)Math.Min((double)actual / goal * 100, 100) : 0;

            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = 100;
            ProgressBar.Value = percent;

            UpdateProgressBarColor(percent);
        }

        private void UpdateProgressBarColor(int percent)
        {
            if (percent >= 100)
            {
                ProgressBar.ProgressColorA = Color.FromArgb(34, 139, 34);
                ProgressBar.ProgressColorB = Color.FromArgb(0, 200, 0);
                ShowGoalReachedMessage();
            }
            else if (percent >= 60)
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
            if (_goalMessageShown) return;

            _goalMessageShown = true;
            MessageBox.Show(
                $"🎉 Congratulations, {_data.User.FirstName}!\nYou've reached your daily earnings goal!",
                "Goal Reached!",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void LoadDataGrid()
        {
            DataGridTripHistory.DataSource = _tripRepo.GetTripHistory(_data.Driver.DriverID);
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

        private async Task LoadRouteToMap()
        {
            if (!_mapLoaded) return;

            var driver = _dashboardService.GetDriver(_userID);
            if (driver == null) return;

            var route = _dashboardService.GetDriverRouteByDriverID(driver.DriverID);
            if (route == null) return;

            _routeId = route.RouteID;

            var result = await _routeService.GetTrafficAndDuration(
                route.StartLat, route.StartLng,
                route.EndLat, route.EndLng
            );

            var payload = new object[]
            {
                new[] { route.StartLng, route.StartLat },
                new[] { route.EndLng,   route.EndLat   },
                route.RouteName
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

        // join queue button
        private void btnJoinQueue_Click(object sender, EventArgs e)
        {
            var driver = _dashboardService.GetDriver(_userID);
            if (driver == null) return;

            var route = _dashboardService.GetDriverRouteByDriverID(driver.DriverID);
            if (route == null) return;

            if (driver.Status == DriverStatus.Finished)
            {
                var driverRepo = new DriverRepository();
                driverRepo.UpdateStatus(driver.DriverID, (int)DriverStatus.Waiting);
            }

            var message = _queueService.JoinQueue(driver.DriverID, route.RouteID);
            MessageBox.Show(message,
                    "Queue Update",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

            UpdateJoinButtonState(driver.DriverID, route.RouteID);

            foreach (Form f in Application.OpenForms)
            {
                if (f is DriverViewQueue viewQueue)
                {
                    viewQueue.UpdateStartButtonState();
                    break;
                }
            }
        }

        private void UpdateJoinButtonState(int driverID, int routeID)
        {
            var driver = _dashboardService.GetDriver(_userID);
            if (driver == null) return;

            bool alreadyInQueue = _queueService.IsDriverInQueue(driverID, routeID);

            bool canJoin = !alreadyInQueue && (driver.Status == DriverStatus.Waiting ||
                            driver.Status == DriverStatus.Finished);

            btnJoinQueue.Enabled = canJoin;
            btnJoinQueue.Text = canJoin ? "Join Queue" : "Unavailable";
            btnJoinQueue.FillColor = canJoin
                ? Color.FromArgb(55, 91, 231)
                : Color.Gray;
        }

        public void RefreshJoinButton()
        {
            var driver = _dashboardService.GetDriver(_userID);
            if (driver == null) return;

            var route = _dashboardService.GetDriverRouteByDriverID(driver.DriverID);
            if (route == null) return;

            UpdateJoinButtonState(driver.DriverID, route.RouteID);
        }

        // navigation

        // view queue navbar button
        private async void ViewQueueBtn_Click(object sender, EventArgs e)
        {
            if (_routeId == 0)
            {
                MessageBox.Show("Route not loaded yet.");
                return;
            }

            DriverViewQueue viewQueue = new DriverViewQueue(_routeId, _userID);
            await FormAnimator.SwitchAsync(this, viewQueue);
        }

        // settings navbar button
        private async void DriverSettingsBtn_Click(object sender, EventArgs e)
        {
            var settings = new DriverSettings(_userID);
            settings.StartPosition = FormStartPosition.Manual;
            settings.Location = this.Location;
            settings.Size = this.Size;

            await FormAnimator.SwitchAsync(this, settings);
        }

        // logout button
        private async void LogoutBtn_Click(object sender, EventArgs e)
        {
            var authService = new AuthenticationService();
            authService.Logout(_userID);
            await FormAnimator.SwitchAsync(this, new LoginForm());
        }
    }
}