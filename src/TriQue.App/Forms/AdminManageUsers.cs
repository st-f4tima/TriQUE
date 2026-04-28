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
    public partial class AdminManageUsers : Form
    {
        public AdminManageUsers()
        {
            InitializeComponent();
        }

        private void ViewQueueBtn_Click(object sender, EventArgs e)
        {
            AdminViewQueue adminForm = new AdminViewQueue();
            adminForm.Show();
            this.Hide();
        }


        private void GenerateReportBtn_Click(object sender, EventArgs e)
        {
            AdminGenerateReport reportForm = new AdminGenerateReport();
            reportForm.Show();
            this.Hide();
        }

        private void SettingsBtn_Click(object sender, EventArgs e)
        {
            AdminSettings settingsForm = new AdminSettings();
            settingsForm.Show();
            this.Hide();
        }

        private void DashBtn_Click(object sender, EventArgs e)
        {
            AdminViewQueue adminForm = new AdminViewQueue();
            adminForm.Show();
            this.Hide();
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            LoginForm adminForm = new LoginForm();
            adminForm.Show();
            this.Hide();
        }
    }
}
