using System.Drawing;
using System.Windows.Forms;
using TriQue.Data.Repositories;
using TriQue.Helpers.Animation;
using TriQue.Models;
using TriQue.Services;

namespace TriQue.Forms
{
    public partial class DriverSettings : Form
    {
        private int _userID;
        private readonly DriverRepository _driverRepo;
        private RotationService _rotationService;

        public DriverSettings(int userID)
        {
            InitializeComponent();
            _userID = userID;
            _driverRepo = new DriverRepository();
            _rotationService = new RotationService();

            LoadDriverInfo();
        }

        private void LoadDriverInfo()
        {
            var info = _driverRepo.GetDriverSettings(_userID);
            if (info == null) return;

            var driver = _driverRepo.GetByUserID(_userID);
            if (driver == null) return;

            var todayRoute = _rotationService.GetTodayRoute(driver.GroupID);

            lblDriverName.Text = info.Value.FullName;
            lblBodyNumber.Text = "Body No. " + info.Value.BodyNumber;
            lblContactNumberValue.Text = info.Value.PhoneNumber;
            lblAssignedRouteValue.Text = todayRoute?.RouteName ?? "No Route";
            lblGroupNameValue.Text = info.Value.GroupName;
            lblRoleValue.Text = "Driver";
            lblCurrentStatusValue.Text = info.Value.StatusName;

            switch (lblCurrentStatusValue.Text)
            {
                case "OnTrip":
                    StatusPanel.FillColor = Color.FromArgb(40, 167, 69); 
                    break;

                case "Waiting":
                    StatusPanel.FillColor = Color.FromArgb(255, 193, 7);
                    break;

                case "Finished":
                    StatusPanel.FillColor = Color.FromArgb(0, 123, 255); 
                    break;

                default:
                    StatusPanel.FillColor = Color.Gray; 
                    break;
            }
        }

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
            var authService = new AuthenticationService();
            authService.Logout(_userID);
            await FormAnimator.SwitchAsync(this, new LoginForm(), closeCurrentAfter: true);
        }
    }
}