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
        private int _userID;

        public DriverForm(int userID)
        {
            InitializeComponent();
            _userID = userID;
            _dashboardService = new DriverDashboardService();
            _queueRepo = new QueueRepository();
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            _data = _dashboardService.GetDashboard(_userID);
            DisplayData();
        }

        private void DisplayData()
        {
            // first name
            textBox1.Text = $"Welcome Back, {_data.User.FirstName}!";

            // actual earnings

            textBox5.Text = _data.ActualEarnings.ToString("₱ 0");

            // goal earnings
            textBox3.Text = $"Goal: {_data.GoalEarnings.ToString("₱ 0")}";

            // progress bar
            int goal = (int)_data.GoalEarnings;
            int actual = (int)_data.ActualEarnings;

            progressBar1.Minimum = 0;
            progressBar1.Maximum = goal > 0 ? goal : 1;
            progressBar1.Value = Math.Min(actual, goal);

            // total completed trips
            textBox6.Text = _data.CompletedTrips.ToString();

            // total trips today
            textBox9.Text = _data.TodayTrips.ToString();

            // fastest trip
            textBox12.Text = $"{_data.FastestTrip:0} min";

            // slowest trip
            textBox13.Text = $"{_data.SlowestTrip:0} min";

            guna2DataGridView1.DataSource = _queueRepo.GetQueueHistory(_data.Driver.DriverID);

        }


        //private void textBox1_TextChanged(object sender, EventArgs e)
        //{

        //}

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await webView21.EnsureCoreWebView2Async();

            string html = @"
    <html>
    <body style='margin:0'>
        <iframe 
            src='https://www.google.com/maps/embed?pb=!1m10!1m8!1m3!1d3874.8815970302435!2d121.070066!3d13.7860105!3m2!1i1024!2i768!4f13.1!5e0!3m2!1sen!2sph!4v1776747251447!5m2!1sen!2sph'
            width='100%' 
            height='100%' 
            style='border:0;' 
            allowfullscreen='' 
            loading='lazy'>
        </iframe>
    </body>
    </html>";

            webView21.NavigateToString(html);
        }

        private void guna2Panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void webView21_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Close();
        }

        private void guna2ImageButton5_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {

        }

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

        private Form DriverViewQueue;

        public DriverForm(Form form)
        {
            InitializeComponent();
            DriverViewQueue = form;
        }
    }
}
