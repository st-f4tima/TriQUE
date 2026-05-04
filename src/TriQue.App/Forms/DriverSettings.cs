using System.Drawing;
using System.Windows.Forms;
using TriQue.Data.Repositories;
using TriQue.Models;
using TriQue.Services;

namespace TriQue.Forms
{
    public partial class DriverSettings : Form
    {
        private int _userID;
        private readonly DriverRepository _driverRepo;

        public DriverSettings(int userID)
        {
            InitializeComponent();
            _userID = userID;
            _driverRepo = new DriverRepository();

            LoadDriverInfo();
        }

        private void LoadDriverInfo()
        {
            var info = _driverRepo.GetDriverSettings(_userID);
            if (info == null) return;

            lblDriverName.Text = info.Value.FullName;
            lblBodyNumber.Text = "Body No. " + info.Value.BodyNumber;
            lblContactNumberValue.Text = info.Value.PhoneNumber;
            lblAssignedRouteValue.Text = info.Value.RouteName;
            lblGroupNameValue.Text = info.Value.GroupName;
            lblRoleValue.Text = "Driver";
            lblCurrentStatusValue.Text = info.Value.StatusName;
        }

        private void DashBtn_Click(object sender, EventArgs e)
        {
            DriverForm dash = new DriverForm(_userID);
            dash.Show();
            this.Close();
        }

        private void ViewQueueBtn_Click(object sender, EventArgs e)
        {
            var dashboard = new DriverDashboardService();
            var driver = dashboard.GetDriver(_userID);
            if (driver == null) return;

            var route = dashboard.GetDriverRouteByDriverID(driver.DriverID);
            if (route == null) return;

            DriverViewQueue viewQueue = new DriverViewQueue(route.RouteID, _userID);
            viewQueue.Show();
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