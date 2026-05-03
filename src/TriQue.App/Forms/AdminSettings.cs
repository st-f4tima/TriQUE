using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TriQue.Forms;

namespace Trique.Forms
{
    public partial class AdminSettings : Form
    {
        private int _userID;
        public AdminSettings(int userID)
        {
            InitializeComponent();
            _userID = userID;
        }

        private void GenerateReportBtn_Click(object sender, EventArgs e)
        {
            AdminGenerateReport reportForm = new AdminGenerateReport(_userID);
            reportForm.Show();
            this.Hide();
        }

        private void ViewQueueBtn_Click(object sender, EventArgs e)
        {
            AdminViewQueue adminForm = new AdminViewQueue(_userID);
            adminForm.Show();
            this.Hide();
        }

        private void ManageUsersBtn_Click(object sender, EventArgs e)
        {
            AdminManageUsers adminForm = new AdminManageUsers(_userID);
            adminForm.Show();
            this.Hide();
        }

        private void DashBtn_Click(object sender, EventArgs e)
        {
            AdminViewQueue adminForm = new AdminViewQueue(_userID);
            adminForm.Show();
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
