using TriQue.Data.Repositories;
using TriQue.DTOs;
using TriQue.Enums;
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
            lblTodayEarningValue.Text = _data.ActualEarnings.ToString("₱ 0");
            lblEarningsGoal.Text = $"Goal: {_data.Driver.GoalEarnings.ToString("₱ 0")}";

            // progress bar
            int goal = (int)_data.Driver.GoalEarnings;
            int actual = (int)_data.ActualEarnings;

            progressBar1.Minimum = 0;
            progressBar1.Maximum = goal > 0 ? goal : 1;
            progressBar1.Value = Math.Min(actual, progressBar1.Maximum);

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

            string path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Assets", "map.html"
            );

            webView21.Source = new Uri(path);
            webView21.NavigationCompleted += async (s, args) =>
            {
                await LoadRouteToMap();
            };
        }

        private async Task LoadRouteToMap()
        {
            var driver = _dashboardService.GetDriver(_userID);
            if (driver == null) return;

            var route = _dashboardService.GetDriverRouteByDriverID(driver.DriverID);
            if (route == null) return;

            _routeId = route.RouteID;
            var result = await _routeService.GetTrafficAndDuration(
                route.StartLat, route.StartLng,
                route.EndLat, route.EndLng
            );

            var coords = new[]
            {
                new[] { route.StartLng, route.StartLat },
                new[] { route.EndLng,   route.EndLat   }
            };

            string json = System.Text.Json.JsonSerializer.Serialize(coords);
            await webView21.CoreWebView2.ExecuteScriptAsync($"drawRoute({json});");

            // display
            lblTrafficStatus.Text = $"{result.trafficCondition}";
            if (result.trafficCondition == "Light")
            {
                lblTrafficStatus.ForeColor = Color.Green;
            }
            else if (result.trafficCondition == "Moderate")
            {
                lblTrafficStatus.ForeColor = Color.Orange;
            }
            else if (result.trafficCondition == "Heavy")
            {
                lblTrafficStatus.ForeColor = Color.Red;
            }

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
            MessageBox.Show(message);

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
        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            if (_routeId == 0)
            {
                MessageBox.Show("Route not loaded yet.");
                return;
            }

            DriverViewQueue viewQueue = new DriverViewQueue(_routeId, _userID);
            viewQueue.Show();
            this.Hide();
        }

        // settings navbar button
        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            DriverSettings settings = new DriverSettings(_userID);
            settings.Show();
            this.Hide();
        }

        // logout button
        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Close();
        }
    }
}