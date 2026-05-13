using Guna.Charts.WinForms;
using System.Data;
using TriQue.Data.Repositories;
using TriQue.Helpers.Animation;
using TriQue.Services;

namespace TriQue.Forms
{
    public partial class AdminForm : Form
    {
        private readonly TrafficService _trafficService = new();
        private readonly AdminService _adminService = new();
        private readonly TripService _tripService = new();
        private readonly DriverService _driverService = new();
        
        private System.Windows.Forms.Timer _refreshTimer;
        
        private int _userID;

        public AdminForm(int userID)
        {
            InitializeComponent();
            _userID = userID;
            SetupRefreshTimer();
            this.Load += AdminForm_Load;
        }

        private async void AdminForm_Load(object? sender, EventArgs e)
        {
            LoadCharts();
            LoadTripStats();
            await LoadTrafficData();
        }

        private void LoadTripStats()
        {
            TotalTripsValue.Text = _tripService.GetTotalTrips();
            HighestTripsValue.Text = _tripService.GetHighestTripsRoute().route;
            LowestTripsValue.Text = _tripService.GetLowestTripsRoute().route;
        }

        private async Task LoadTrafficData()
        {
            try
            {
                var trafficList = await _trafficService.GetAllRouteTrafficAsync();

                var worst = trafficList
                    .OrderByDescending(t => t.DelaySec)
                    .FirstOrDefault();

                if (worst != null)
                {
                    TrafficProneRouteValue.Text = worst.IsTrafficProne
                        ? worst.RouteName
                        : "None Detected";

                    PeakCongestionDurationValue.Text = worst.PeakWindow;
                }
            }
            catch (Exception ex)
            {
                TrafficProneRouteValue.Text = "Unavailable";
                PeakCongestionDurationValue.Text = "Unavailable";
                Console.WriteLine($"[Traffic] {ex.Message}");
            }
        }

        #region Charts

        private void LoadCharts()
        {
            LoadPieChart();
            LoadBarChart();
        }

        private void LoadPieChart()
        {
            PieChart.Datasets.Clear();

            var status = _driverService.GetDriverStatusDistribution();
            var pieDataset = new GunaPieDataset();

            string[] order = { "Waiting", "OnTrip", "Finished" };
            Color[] colors = {
                Color.FromArgb(255, 193,   7),
                Color.FromArgb( 55,  91, 231),
                Color.FromArgb( 40, 167,  69)
            };

            for (int i = 0; i < order.Length; i++)
            {
                string key = order[i];
                int count = status.ContainsKey(key) ? status[key] : 0;

                pieDataset.DataPoints.Add(key, count);
                pieDataset.FillColors.Add(colors[i]);
            }

            PieChart.Datasets.Add(pieDataset);
            PieChart.XAxes.Display = false;
            PieChart.YAxes.Display = false;
            PieChart.Update();
        }

        private void LoadBarChart()
        {
            BarGraph.Datasets.Clear();

            var routes = _driverService.GetDriversPerRoute();
            var barDataset = new GunaBarDataset();

            Color[] colors = {
                Color.FromArgb(55,  91, 231),
                Color.FromArgb(55,  91, 231),
                Color.FromArgb(55,  91, 231),
                Color.FromArgb(55,  91, 231),
                Color.FromArgb(55,  91, 231),
                Color.FromArgb(55,  91, 231),
            };

            int i = 0;
            foreach (var kvp in routes)
            {
                barDataset.DataPoints.Add(kvp.Key, kvp.Value);
                barDataset.FillColors.Add(colors[i % colors.Length]);
                i++;
            }

            BarGraph.Datasets.Add(barDataset);
            BarGraph.XAxes.GridLines.Display = false;
            BarGraph.YAxes.GridLines.Display = false;
            BarGraph.Legend.Display = false;
            BarGraph.Update();
        }

        private void SetupRefreshTimer()
        {
            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = 30 * 60 * 1000;
            _refreshTimer.Tick += async (s, e) => await LoadTrafficData();
            _refreshTimer.Start();
        }

        #endregion

        #region navigation

        private async void DashboardBtn_Click(object sender, EventArgs e)
        {
            await FormAnimator.SwitchAsync(this, new AdminForm(_userID));
        }

        private async void ViewQueue_Click(object sender, EventArgs e)
        {
            await FormAnimator.SwitchAsync(this, new AdminViewQueue(_userID));
        }

        private async void SettingsBtn_Click(object sender, EventArgs e)
        {
            await FormAnimator.SwitchAsync(this, new AdminSettings(_userID));
        }

        private async void ManageUsersBtn_Click(object sender, EventArgs e)
        {
            var repo = new UserRepository();
            int level = repo.GetAdminLevel(_userID);

            if (level != 1)
            {
                MessageBox.Show(
                    "Access denied. Only SuperAdmins can manage users.",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            await FormAnimator.SwitchAsync(this, new AdminManageUsers(_userID));
        }

        private async void GenerateReportBtn_Click(object sender, EventArgs e)
        {
            var repo = new UserRepository();
            int level = repo.GetAdminLevel(_userID);

            if (level != 1 && level != 2)
            {
                MessageBox.Show(
                    "Access denied. Only SuperAdmins and Toda Officers can manage users.",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            await FormAnimator.SwitchAsync(this, new AdminGenerateReport(_userID));
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