using TriQue.Data.Repositories;
using TriQue.Forms;

namespace Trique.Forms
{
    public partial class AdminViewQueue : Form
    {
        private readonly int _userID;
        public AdminViewQueue(int userID)
        {
            InitializeComponent();
            _userID = userID;
        }

        private void btnRouteA_Click(object sender, EventArgs e)
        {
            QueueModal modal = new QueueModal("Provincial Capitol", 101, _userID);
            modal.ShowDialog();
        }

        private void btnRouteB_Click(object sender, EventArgs e)
        {
            QueueModal modal = new QueueModal("Grand Terminal", 102, _userID);
            modal.ShowDialog();
        }

        private void btnRouteC_Click(object sender, EventArgs e)
        {
            QueueModal modal = new QueueModal("SM Batangas", 103, _userID);
            modal.ShowDialog();
        }

        private void btnRouteD_Click(object sender, EventArgs e)
        {
            QueueModal modal = new QueueModal("WalterMart", 104, _userID);
            modal.ShowDialog();
        }

        private void btnRouteE_Click(object sender, EventArgs e)
        {
            QueueModal modal = new QueueModal("Brgy. Tulo", 105, _userID);
            modal.ShowDialog();
        }

        private void btnRouteF_Click(object sender, EventArgs e)
        {
            QueueModal modal = new QueueModal("BSU Alangilan", 106, _userID);
            modal.ShowDialog();
        }

        // navbar
        private void DashBtn_Click(object sender, EventArgs e)
        {
            AdminForm adminForm = new AdminForm(_userID);
            adminForm.Show();
            this.Hide();
        }

        private void ManageUserBtn_Click(object sender, EventArgs e)
        {
            var repo = new UserRepository();
            int level = repo.GetAdminLevel(_userID);

            // 1 = SuperAdmin only
            if (level != 1)
            {
                MessageBox.Show(
                    "Access denied. Only SuperAdmins can manage users.",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            AdminManageUsers adminManageUsers = new AdminManageUsers(_userID);
            adminManageUsers.Show();
            this.Hide();
        }

        private void GenerateReportBtn_Click(object sender, EventArgs e)
        {
            AdminGenerateReport reportForm = new AdminGenerateReport(_userID);
            reportForm.Show();
            this.Hide();
        }

        private void SettingsBtn_Click(object sender, EventArgs e)
        {
            AdminSettings settingsForm = new AdminSettings(_userID);
            settingsForm.Show();
            this.Hide();
        }

        private void LogoutBtn_Click(object sender, EventArgs e)
        {
            LoginForm adminForm = new LoginForm();
            adminForm.Show();
            this.Hide();
        }
    }
}
