using System;
using System.Drawing;
using System.Windows.Forms;
using TriQue.Data.Repositories;
using TriQue.Enums;

namespace TriQue.Forms
{
    public partial class DriverViewQueue : Form
    {
        private Form previousForm;
        private QueueRepository _queueRepo;
        private DriverRepository _driverRepo;
        private TripRepository _tripRepo;

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
            _tripRepo = new TripRepository();

            var driver = _driverRepo.GetByUserID(_userID);
            if (driver != null)
                _driverID = driver.DriverID;

            _queueId = _queueRepo.GetQueueIdByRouteId(_routeId);

            displayData();
        }

        private void displayData()
        {
            int queueId = _queueRepo.GetQueueIdByRouteId(_routeId);

            var driverRepo = new DriverRepository();
            var driver = driverRepo.GetByUserID(_userID);

            if (driver == null) return;

            var row = _queueRepo.GetQueueDriver(queueId, driver.DriverID);

            if (row != null)
            {
                textBox6.Text = row["Position"].ToString();
                textBox5.Text = row["RouteName"].ToString();
                textBox4.Text = row["Status"].ToString();
            }

            DataGridQueueStatus.DataSource =
                _queueRepo.GetQueueDrivers(queueId);
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            displayData();
        }



        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (_driverID == 0)
            {
                MessageBox.Show("Driver not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var driver = _driverRepo.GetByDriverID(_driverID);
            if (driver == null) return;

            // ✅ Compare against enum, not magic int
            if (driver.Status != DriverStatus.Waiting)
            {
                MessageBox.Show("You can only start a trip when your status is Waiting.",
                    "Cannot Start", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                "Start your trip now?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            // 1. Update driver status → OnTrip
            _driverRepo.UpdateStatus(_driverID, (int)DriverStatus.OnTrip);

            // 2. Insert new Trip row
            _tripRepo.StartTrip(_driverID, _routeId);

            // 3. Refresh UI
            displayData();

            MessageBox.Show("Trip started! Your status is now On Trip.",
                "Trip Started", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

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