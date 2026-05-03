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
using TriQue.Forms;
using TriQue.Services;
using TriQue.Data.Repositories;
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

        // navigation
        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            new LoginForm().Show();
            this.Hide();
        }

        private void DashboardBtn_Click(object sender, EventArgs e) {
            AdminForm adminForm = new AdminForm(_userID);
            adminForm.Show();
            this.Hide();
        }

        private void ViewQueue_Click(object sender, EventArgs e)
        {
            AdminViewQueue adminViewQueue = new AdminViewQueue(_userID);
            adminViewQueue.Show();
            this.Hide();
        }

        private void SettingsBtn_Click(object sender, EventArgs e)
        {
            AdminSettings adminSettings = new AdminSettings(_userID);
            adminSettings.Show();
            this.Hide();
        }

        private void ManageUsersBtn_Click(object sender, EventArgs e)
        {
            AdminManageUsers adminManageUsers = new AdminManageUsers(_userID);
            adminManageUsers.Show();
            this.Hide();
        }
    }
}