using TriQue.Enums;
using TriQue.Helpers.Animation;
using TriQue.Services;

namespace TriQue.Forms
{
    public partial class DriverViewQueue : Form
    {
        private readonly QueueService _queueService = new();
        private readonly DriverService _driverService = new();
        private readonly TripService _tripService = new();
        private readonly RotationService _rotationService = new();

        private readonly int _routeID;
        private readonly int _userID;

        private int _driverID;
        private int _queueID;

        public DriverViewQueue(int routeID, int userID)
        {
            InitializeComponent();

            _routeID = routeID;
            _userID = userID;
            _driverID = _driverService.GetDriverId(userID) ?? 0;
            _queueID = _queueService.GetQueueIdByRouteId(routeID) ?? 0;

            LoadView();
        }

        #region load and data

        private void LoadView()
        {
            DisplayData();
            UpdateStartButtonState();
        }

        private void DisplayData()
        {
            if (_driverID == 0) return;

            var row = _queueService.GetQueueDriver(_queueID, _driverID);
            var driver = _driverService.GetByDriverId(_driverID);

            if (row != null)
            {
                lblRankingValue.Text = row["Position"]?.ToString() ?? "—";
                ApplyStatus(row["Status"]?.ToString() ?? "");
            }
            else
            {
                lblRankingValue.Text = "—";
                ApplyStatus(driver?.Status.ToString() ?? "Unknown");
            }

            if (driver != null)
            {
                var todayRoute = _rotationService.GetTodayRoute(driver.GroupID);
                lblRouteValue.Text = todayRoute?.RouteName ?? "-";
            }
            else
            {
                lblRouteValue.Text = "-";
            }

            DataGridQueueStatus.DataSource = _queueService.GetQueueDrivers(_queueID);
            ApplyGridStyles();
        }

        #endregion

        #region grid styles

        private void ApplyGridStyles()
        {
            if (DataGridQueueStatus.Columns.Count == 0) return;

            if (DataGridQueueStatus.Columns.Contains("DriverName"))
                DataGridQueueStatus.Columns["DriverName"].HeaderText = "Driver Name";

            if (DataGridQueueStatus.Columns.Contains("BodyNumber"))
                DataGridQueueStatus.Columns["BodyNumber"].HeaderText = "Body Number";

            if (DataGridQueueStatus.Columns.Contains("Position"))
                DataGridQueueStatus.Columns["Position"].HeaderText = "Position";

            if (DataGridQueueStatus.Columns.Contains("Status"))
                DataGridQueueStatus.Columns["Status"].HeaderText = "Status";
        }

        private void DataGridQueueStatus_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            if (DataGridQueueStatus.Columns[e.ColumnIndex].Name == "Status")
            {
                string status = e.Value.ToString();

                e.CellStyle.ForeColor = status switch
                {
                    "Waiting" => Color.FromArgb(255, 193, 7),
                    "OnTrip" => Color.FromArgb(40, 167, 69),
                    "Finished" => Color.FromArgb(0, 123, 255),
                    _ => Color.Gray
                };

                if (status == "OnTrip")
                {
                    e.Value = "On Trip";
                    e.FormattingApplied = true;
                }

                e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                e.CellStyle.BackColor = Color.White;
                e.CellStyle.SelectionBackColor = Color.FromArgb(240, 240, 240);
                e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
            }
        }

        #endregion

        #region status UI

        private void ApplyStatus(string status)
        {
            lblStatusValue.Text = FormatStatus(status);
            lblStatusValue.ForeColor = GetStatusColor(status);
        }

        private string FormatStatus(string status)
        {
            return status switch
            {
                "OnTrip" => "On Trip",
                "Waiting" => "Waiting",
                "Finished" => "Finished",
                _ => status
            };
        }

        private Color GetStatusColor(string status)
        {
            return status switch
            {
                "Waiting" => Color.FromArgb(255, 193, 7),
                "OnTrip" => Color.FromArgb(40, 167, 69),
                "Finished" => Color.FromArgb(0, 123, 255),
                _ => Color.Gray
            };
        }

        #endregion

        #region trip action

        private void StartTripBtn_Click(object sender, EventArgs e)
        {
            var driver = _driverService.GetByDriverId(_driverID);
            if (driver == null) return;

            bool inQueue = IsDriverInQueue();

            // start trip
            if (driver.Status == DriverStatus.Waiting && inQueue)
            {
                var confirm = MessageBox.Show(
                    "Start your trip now?",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) return;

                _tripService.StartTrip(_driverID, _routeID);
                _driverService.UpdateStatus(_driverID, DriverStatus.OnTrip);

                RefreshUI();
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

                _tripService.EndTrip(_driverID, _routeID);

                _driverService.UpdateStatus(_driverID, DriverStatus.Finished);
                _queueService.RemoveDriverFromQueue(_driverID, _queueID);
                _queueService.ReorderQueuePositions(_queueID);

                RefreshUI();
                NotifyDashboard();
                return;
            }
        }

        #endregion

        #region UI state

        private void RefreshUI()
        {
            DisplayData();
            UpdateStartButtonState();
        }

        public void UpdateStartButtonState()
        {
            var driver = _driverService.GetByDriverId(_driverID);
            if (driver == null) return;

            bool inQueue = IsDriverInQueue();

            switch (driver.Status)
            {
                case DriverStatus.Waiting:
                    StartTripBtn.Text = "Start Trip";
                    StartTripBtn.FillColor = inQueue
                        ? Color.FromArgb(55, 91, 231)
                        : Color.Gray;
                    StartTripBtn.Enabled = inQueue;
                    break;

                case DriverStatus.OnTrip:
                    StartTripBtn.Text = "End Trip";
                    StartTripBtn.FillColor = Color.Red;
                    StartTripBtn.Enabled = true;
                    break;

                case DriverStatus.Finished:
                    StartTripBtn.Text = "Start Trip";
                    StartTripBtn.FillColor = Color.Gray;
                    StartTripBtn.Enabled = false;
                    break;
            }
        }

        private bool IsDriverInQueue()
        {
            return _queueService.IsDriverInQueue(_driverID, _routeID);
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            RefreshUI();
        }

        private void NotifyDashboard()
        {
            foreach (Form f in Application.OpenForms)
            {
                if (f is DriverForm dash)
                {
                    dash.RefreshJoinButton();
                    break;
                }
            }
        }


        #endregion

        #region navigation

        private async void DashBtn_Click(object sender, EventArgs e)
        {
            await FormAnimator.SwitchAsync(this, new DriverForm(_userID), true);
        }

        private async void SettingsBtn_Click(object sender, EventArgs e)
        {
            await FormAnimator.SwitchAsync(this, new DriverSettings(_userID), true);
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