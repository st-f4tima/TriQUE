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
    public partial class AdminViewQueue : Form
    {
        public AdminViewQueue()
        {
            InitializeComponent();
        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnRouteA_Click(object sender, EventArgs e)
        {
            QueueModal modal = new QueueModal("Route A");
            modal.ShowDialog();
        }

        private void btnRouteB_Click(object sender, EventArgs e)
        {
            QueueModal modal = new QueueModal("Route B");
            modal.ShowDialog();
        }

        private void btnRouteC_Click(object sender, EventArgs e)
        {
            QueueModal modal = new QueueModal("Route C");
            modal.ShowDialog();
        }

        private void btnRouteD_Click(object sender, EventArgs e)
        {
            QueueModal modal = new QueueModal("Route D");
            modal.ShowDialog();
        }

        private void btnRouteE_Click(object sender, EventArgs e)
        {
            QueueModal modal = new QueueModal("Route E");
            modal.ShowDialog();
        }

        private void btnRouteF_Click(object sender, EventArgs e)
        {
            QueueModal modal = new QueueModal("Route F");
            modal.ShowDialog();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void ManageUserBtn_Click(object sender, EventArgs e)
        {
            AdminManageUsers adminForm = new AdminManageUsers();
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

        private void LogoutBtn_Click(object sender, EventArgs e)
        {
            LoginForm adminForm = new LoginForm();
            adminForm.Show();
            this.Hide();
        }
    }
}
