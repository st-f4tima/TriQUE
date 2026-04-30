using System.Drawing;
using System.Windows.Forms;
using Trique.Forms;
using TriQue.Data;
using TriQue.Data.Repositories;
using TriQue.Models;
using TriQue.Services;

namespace TriQue.Forms
{
    public partial class DriverSettings : Form
    {
        private Form previousForm;
        private int _userID;

        public DriverSettings(int userID)
        {
            InitializeComponent();
            _userID = userID;
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