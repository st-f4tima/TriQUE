using TriQue.Forms;

namespace Trique.Forms
{
    public partial class AdminManageUsers : Form
    {
        private int _userID;

        public AdminManageUsers(int userID)
        {
            InitializeComponent();
            _userID = userID;
        }

        private void DashboardBtn_Click(object sender, EventArgs e)
        {
            AdminForm adminForm = new AdminForm(_userID);
            adminForm.Show();
            this.Hide();

        }

        private void ViewQueueBtn_Click_1(object sender, EventArgs e)
        {
            AdminViewQueue adminViewQueue = new AdminViewQueue(_userID);
            adminViewQueue.Show();
            this.Hide();
        }

        private void GenerateReportBtn_Click_1(object sender, EventArgs e)
        {
            AdminGenerateReport adminGenerateReport = new AdminGenerateReport(_userID);
            adminGenerateReport.Show();
            this.Hide();
        }

        private void SettingsBtn_Click(object sender, EventArgs e)
        {
            AdminSettings adminSettings = new AdminSettings(_userID);
            adminSettings.Show();
            this.Hide();

        }

        private void ManageUsersBtn_Click(object sender, EventArgs e)
        {
            AdminManageUsers adminManageUsers = new AdminManageUsers(_userID);
            adminManageUsers.Show();
            this.Hide();
        }

        private void LogoutBtn_Click(object sender, EventArgs e)
        {
            new LoginForm().Show();
            this.Hide();
        }
    }
}