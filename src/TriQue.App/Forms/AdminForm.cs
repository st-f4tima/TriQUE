using Guna.Charts.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TriQue.Data.Repositories;
using TriQue.Forms;
using TriQue.Helpers.Animation;
using TriQue.Services;
namespace Trique.Forms
{
    public partial class AdminForm : Form
    {
        private readonly TrafficService _trafficService = new();
        private readonly AdminRepository _adminRepo = new();
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

        private void LoadCharts()
        {

            LoadPieChart();
            LoadBarChart();
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

        private void LoadTripStats()
        {
            var todayRoute = _adminRepo.GetTotalTripsTodayRoute();
            TotalTripsValue.Text = todayRoute;

            var (highRoute, highCount) = _adminRepo.GetHighestTripsRoute();
            HighestTripsValue.Text = highRoute;

            var (lowRoute, lowCount) = _adminRepo.GetLowestTripsRoute();
            LowestTripsValue.Text = lowRoute;

        }

        private void LoadPieChart()
        {
            PieChart.Datasets.Clear();

            var status = _adminRepo.GetDriverStatusDistribution();

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

            var routes = _adminRepo.GetDriversPerRoute();

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

        // navbar
        private async void LogoutBtn_Click(object sender, EventArgs e)
        {
            var authService = new AuthenticationService();
            authService.Logout(_userID);
            await FormAnimator.SwitchAsync(this, new LoginForm());
        }

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
    }
}