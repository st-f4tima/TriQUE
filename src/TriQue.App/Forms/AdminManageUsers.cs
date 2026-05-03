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

        private void DashBtn_Click(object sender, EventArgs e)
        {
            new AdminForm(_userID).Show();
            this.Hide();
        }

        private void ViewQueueBtn_Click(object sender, EventArgs e)
        {
            new AdminViewQueue(_userID).Show();
            this.Hide();
        }

        private void GenerateReportBtn_Click(object sender, EventArgs e)
        {
            new AdminGenerateReport(_userID).Show();
            this.Hide();
        }

        private void SettingsBtn_Click(object sender, EventArgs e)
        {
            new AdminSettings(_userID).Show();
            this.Hide();
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            new LoginForm().Show();
            this.Hide();
        }
    }
}