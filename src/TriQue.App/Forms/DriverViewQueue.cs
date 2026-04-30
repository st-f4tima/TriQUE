using System;
using System.Drawing;
using System.Windows.Forms;
using TriQue.Data.Repositories;

namespace TriQue.Forms
{
    public partial class DriverViewQueue : Form
    {
        private Form previousForm;
        private QueueRepository _queueRepo;

        private int _routeId;
        private int _userID;

        public DriverViewQueue(int routeId, int userID)
        {
            InitializeComponent();
            _routeId = routeId;
            _userID = userID;
            _queueRepo = new QueueRepository();

            displayData();
        }

        private void InitializeContext()
        {
            _queueRepo = new QueueRepository();
            displayData();
        }

        private void displayData()
        {
            int queueId = _queueRepo.GetQueueIdByRouteId(_routeId);

            DataGridQueueStatus.DataSource =
                _queueRepo.GetQueueDrivers(queueId);
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