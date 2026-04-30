using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TriQue.Data.Repositories;
using TriQue.Models;
using TriQue.Services;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TriQue.Forms
{
    public partial class DriverForm : Form
    {
        private DriverDashboardService _dashboardService;
        private DriverDashboardData _data;
        private QueueRepository _queueRepo;
        private RouteService _routeService;
        private QueueService _queueService;
        private int _userID;

        public DriverForm(int userID)
        {
            InitializeComponent();
            InitializeServices();

            _userID = userID;

            LoadDashboard();
        }

        public DriverForm(Form form)
        {
            InitializeComponent();
            InitializeServices();
        }

        private void InitializeServices()
        {
            _dashboardService = new DriverDashboardService();
            _queueRepo = new QueueRepository();
            _routeService = new RouteService();
            _queueService = new QueueService();
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
                    UpdateJoinButtonState(driver.DriverID, route.RouteID);
                }
            }
        }

        private void DisplayData()
        {
            if (_data == null || _data.User == null || _data.Driver == null)
                return;

            // name
            textBox1.Text = $"Welcome Back, {_data.User.FirstName}!";

            // earnings
            textBox5.Text = _data.ActualEarnings.ToString("₱ 0");

            // goal 
            textBox3.Text = $"Goal: {_data.Driver.GoalEarnings.ToString("₱ 0")}";

            // progress bar
            int goal = (int)_data.Driver.GoalEarnings;
            int actual = (int)_data.ActualEarnings;

            progressBar1.Minimum = 0;
            progressBar1.Maximum = goal > 0 ? goal : 1;
            progressBar1.Value = Math.Min(actual, progressBar1.Maximum);

            // stats
            textBox6.Text = _data.CompletedTrips.ToString();
            textBox9.Text = _data.TodayTrips.ToString();
            textBox12.Text = $"{_data.FastestTrip:0} min";
            textBox13.Text = $"{_data.SlowestTrip:0} min";

            var driver = _data.Driver.DriverID;

            // route name
            textBox23.Text = $"On Route - {_data.RouteName}";

            // total distance
            textBox22.Text = $"{_data.TotalDistance} km";


            // queue history
            guna2DataGridView1.DataSource =
                _queueRepo.GetQueueHistory(_data.Driver.DriverID);
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
            textBox20.Text = $"{result.trafficCondition}";
            if (result.trafficCondition == "Light")
            {
                textBox20.ForeColor = Color.Green;
            }
            else if (result.trafficCondition == "Moderate")
            {
                textBox20.ForeColor = Color.Orange;
            }
            else if (result.trafficCondition == "Heavy")
            {
                textBox20.ForeColor = Color.Red;
            }

            textBox21.Text = $"{result.durationMin} min";
        }

        // join queue button
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            var driver = _dashboardService.GetDriver(_userID);
            if (driver == null)
            {
                MessageBox.Show("Driver not found.");
                return;
            }

            var route = _dashboardService.GetDriverRouteByDriverID(driver.DriverID);
            if (route == null)
            {
                MessageBox.Show("Route not found.");
                return;
            }

            var message = _queueService.JoinQueue(driver.DriverID, route.RouteID);
            MessageBox.Show(message);

            UpdateJoinButtonState(driver.DriverID, route.RouteID);
        }

        private void UpdateJoinButtonState(int driverID, int routeID)
        {
            bool isInQueue = _queueService.IsDriverInQueue(driverID, routeID);

            guna2Button1.Enabled = !isInQueue;
            guna2Button1.Text = isInQueue ? "Already in Queue" : "Join Queue";

            guna2Button1.FillColor = isInQueue
            ? Color.Gray
            : Color.FromArgb(55, 91, 231);
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel10_Paint(object sender, PaintEventArgs e) { }
        private void webView21_Click(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void guna2Panel3_Paint(object sender, PaintEventArgs e) { }
        private void textBox5_TextChanged(object sender, EventArgs e) { }
        private void textBox9_TextChanged(object sender, EventArgs e) { }
        private void textBox16_TextChanged(object sender, EventArgs e) { }
        private void textBox15_TextChanged(object sender, EventArgs e) { }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Close();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e) { }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            DriverViewQueue viewQueue = new DriverViewQueue(this);
            viewQueue.Show();
            this.Hide();
        }

        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            DriverSettings settings = new DriverSettings(this);
            settings.Show();
            this.Hide();
        }

        //private Form DriverViewQueue;

        //public DriverForm(Form form)
        //{
        //    InitializeComponent();
        //    DriverViewQueue = form;
        //}
    }
}