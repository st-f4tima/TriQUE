using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;
using TriQue.Data.Repositories;
using TriQue.Enums;
using TriQue.Services;

namespace TriQue.Forms
{
    public partial class DriverViewQueue : Form
    {
        private QueueRepository _queueRepo;
        private DriverRepository _driverRepo;
        private TripService _tripService;

        private int _routeId;
        private int _userID;
        private int _driverID;
        private int _queueId;

        public DriverViewQueue(int routeId, int userID)
        {
            InitializeComponent();

            _routeId = routeId;
            _userID = userID;

            _queueRepo = new QueueRepository();
            _driverRepo = new DriverRepository();
            _tripService = new TripService();

            var driver = _driverRepo.GetByUserID(_userID);
            if (driver != null)
            {
                _driverID = driver.DriverID;
            }

            _queueId = _queueRepo.GetQueueIdByRouteId(_routeId);

            displayData();
            UpdateStartButtonState();
        }

        private void displayData()
        {
            var driver = _driverRepo.GetByUserID(_userID);
            if (driver == null) return;

            var row = _queueRepo.GetQueueDriver(_queueId, driver.DriverID);

            if (row != null)
            {
                textBox6.Text = row["Position"].ToString();
                textBox5.Text = row["RouteName"].ToString();
                textBox4.Text = row["Status"].ToString();
            }

            DataGridQueueStatus.DataSource =
                _queueRepo.GetQueueDrivers(_queueId);
        }

        // start/end trip button
        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            var driver = _driverRepo.GetByDriverID(_driverID);
            if (driver == null) return;

            // start trip button
            if (driver.Status == DriverStatus.Waiting && IsDriverInQueue())
            {
                var confirm = MessageBox.Show(
                    "Start your trip now?",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) return;

                _driverRepo.UpdateStatus(_driverID, (int)DriverStatus.OnTrip);
                _tripService.StartTrip(_driverID, _routeId);

                displayData();
                UpdateStartButtonState();
                return;
            }

            // end trip
            if (driver.Status == DriverStatus.OnTrip)
            {
                var confirm = MessageBox.Show(
                    "End your trip now?",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) return;

                _tripService.EndTrip(_driverID, _routeId);
                _driverRepo.UpdateStatus(_driverID, (int)DriverStatus.Finished);
                _queueRepo.RemoveDriverFromQueue(_driverID, _queueId);

                displayData();
                UpdateStartButtonState();

                // notify dashboard
                foreach (Form f in Application.OpenForms)
                {
                    if (f is DriverForm dashboard)
                    {
                        dashboard.RefreshJoinButton();
                        break;
                    }
                }

                return;
            }
        }

        public void UpdateStartButtonState()
        {
            var driver = _driverRepo.GetByUserID(_userID);
            if (driver == null) return;

            bool inQueue = IsDriverInQueue();

            switch (driver.Status)
            {
                case DriverStatus.Waiting:
                    if (inQueue)
                    {
                        StartTipBtn.Text = "Start Trip";
                        StartTipBtn.FillColor = Color.FromArgb(55, 91, 231);
                        StartTipBtn.Enabled = true;
                    }
                    else
                    {
                        StartTipBtn.Text = "Start Trip";
                        StartTipBtn.FillColor = Color.Gray;
                        StartTipBtn.Enabled = false;
                    }
                    break;

                case DriverStatus.OnTrip:
                    StartTipBtn.Text = "End Trip";
                    StartTipBtn.FillColor = Color.Red;
                    StartTipBtn.Enabled = true;
                    break;

                case DriverStatus.Finished:
                    StartTipBtn.Text = "Start Trip";
                    StartTipBtn.FillColor = Color.Gray;
                    StartTipBtn.Enabled = false;
                    break;
            }
        }

        private bool IsDriverInQueue()
        {
            return _queueRepo.IsDriverAlreadyInQueue(_queueId, _driverID);
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            displayData();
            UpdateStartButtonState();
        }

       
        // navigation
        private void DashBtn_Click(object sender, EventArgs e)
        {
            DriverForm dash = new DriverForm(_userID);
            dash.Show();
            this.Close();
        }

        private void SettingsBtn_Click(object sender, EventArgs e)
        {
            DriverSettings settings = new DriverSettings(_userID);
            settings.Show();
            this.Close();
        }

        private void LogoutBtn_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Close();
        }
    }
}