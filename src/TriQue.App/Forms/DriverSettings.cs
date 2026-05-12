using TriQue.Helpers.Animation;
using TriQue.Services;

namespace TriQue.Forms
{
    public partial class DriverSettings : Form
    {
        private readonly DriverService _driverService = new();
        private readonly RotationService _rotationService = new();

        private int _userID;

        public DriverSettings(int userID)
        {
            InitializeComponent();

            _userID = userID;

            LoadDriverInfo();
        }

        private void LoadDriverInfo()
        {
            var info = _driverService.GetDriverSettings(_userID);
            if (info == null) return;

            var driver = _driverService.GetByUserId(_userID);
            if (driver == null) return;

            var todayRoute = _rotationService.GetTodayRoute(driver.GroupID);

            lblDriverName.Text = info.FullName;
            lblBodyNumber.Text = "Body No. " + info.BodyNumber;
            lblContactNumberValue.Text = info.PhoneNumber;
            lblAssignedRouteValue.Text = todayRoute?.RouteName ?? "No Route";
            lblGroupNameValue.Text = info.GroupName;
            lblCurrentStatusValue.Text = info.StatusName;

            SetStatusColor(info.StatusName);
        }

        private void SetStatusColor(string status)
        {
            StatusPanel.FillColor = status switch
            {
                "OnTrip" => Color.FromArgb(40, 167, 69),
                "Waiting" => Color.FromArgb(255, 193, 7),
                "Finished" => Color.FromArgb(0, 123, 255),
                _ => Color.Gray
            };
        }

        #region navigation

        private async void DashBtn_Click(object sender, EventArgs e)
        {
            DriverForm dash = new DriverForm(_userID);
            await FormAnimator.SwitchAsync(this, dash, closeCurrentAfter: true);
        }

        private async void ViewQueueBtn_Click(object sender, EventArgs e)
        {
            var dashboard = new DriverDashboardService();
            var driver = dashboard.GetDriver(_userID);
            if (driver == null) return;

            var route = dashboard.GetDriverRouteByDriverID(driver.DriverID);
            if (route == null) return;

            DriverViewQueue viewQueue = new DriverViewQueue(route.RouteID, _userID);
            await FormAnimator.SwitchAsync(this, viewQueue, closeCurrentAfter: true);
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
    }

    #endregion
}