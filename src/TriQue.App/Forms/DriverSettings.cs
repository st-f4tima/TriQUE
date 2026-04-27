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
        private Form previousForm; // ✅ ONE reference only

        public DriverSettings()
        {
            InitializeComponent();
        }

        // ✅ Constructor for navigation
        public DriverSettings(Form form)
        {
            InitializeComponent();
            previousForm = form;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        // ✅ Go to Dashboard (DriverForm)
        private void DashBtn_Click(object sender, EventArgs e)
        {
            DriverForm dash = new DriverForm(this);
            dash.Show();
            this.Hide();
        }

        private void ViewQueueBtn_Click(object sender, EventArgs e)
        {
            DriverViewQueue viewQueue = new DriverViewQueue(this);
            viewQueue.Show();
            this.Hide();
        }

        private void LogoutBtn_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Close();
        }
    }
}