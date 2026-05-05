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
                lblRankingValue.Text = row["Position"].ToString();

                string status = row["Status"].ToString();

                lblStatusValue.Text = status switch
                {
                    "OnTrip" => "On Trip",
                    "Waiting" => "Waiting",
                    "Finished" => "Finished",
                    _ => status
                };

                lblStatusValue.ForeColor = status switch
                {
                    "OnTrip" => Color.FromArgb(13, 110, 253),
                    "Waiting" => Color.FromArgb(255, 183, 0),
                    "Finished" => Color.FromArgb(0, 200, 83),
                    _ => Color.Gray
                };
            }
            else
            {
                lblRankingValue.Text = "—";

                string fallbackStatus = driver.Status.ToString();

                lblStatusValue.Text = fallbackStatus switch
                {
                    "OnTrip" => "On Trip",
                    "Waiting" => "Waiting",
                    "Finished" => "Finished",
                    _ => fallbackStatus
                };

                lblStatusValue.ForeColor = fallbackStatus switch
                {
                    "OnTrip" => Color.FromArgb(13, 110, 253),
                    "Waiting" => Color.FromArgb(255, 183, 0),
                    "Finished" => Color.FromArgb(0, 200, 83),
                    _ => Color.Gray
                };
            }

            DataGridQueueStatus.DataSource = _queueRepo.GetQueueDrivers(_queueId);
        }

        // start/end trip button
        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            var driver = _driverRepo.GetByDriverID(_driverID);
            if (driver == null) return;

            // --- Start Trip ---
            if (driver.Status == DriverStatus.Waiting && IsDriverInQueue())
            {
                var confirm = MessageBox.Show(
                    "Start your trip now?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                _driverRepo.UpdateStatus(_driverID, (int)DriverStatus.OnTrip);
                _tripService.StartTrip(_driverID, _routeId);

                System.Threading.Thread.Sleep(150);
                displayData();
                UpdateStartButtonState();
                return;
            }

            // --- End Trip ---
            if (driver.Status == DriverStatus.OnTrip)
            {
                var confirm = MessageBox.Show(
                    "End your trip now?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                _tripService.EndTrip(_driverID, _routeId);
                _driverRepo.UpdateStatus(_driverID, (int)DriverStatus.Finished);
                _queueRepo.RemoveDriverFromQueue(_driverID, _queueId);

                // Compact positions: rank 2 → rank 1, rank 3 → rank 2, etc.
                _queueRepo.ReorderQueuePositions(_queueId);

                System.Threading.Thread.Sleep(150);
                displayData();
                UpdateStartButtonState();

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


        // navbar
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