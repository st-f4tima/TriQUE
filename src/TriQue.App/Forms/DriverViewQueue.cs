using System;
using System.Drawing;
using System.Windows.Forms;
using Trique.Forms;
using TriQue.Data;
using TriQue.Data.Repositories;
using TriQue.Models;
using TriQue.Services;

namespace TriQue.Forms
{
    public partial class DriverViewQueue : Form
    {
        private Form previousForm; // ✅ ONLY ONE

        public DriverViewQueue()
        {
            InitializeComponent();
        }

        // ✅ Constructor for navigation
        public DriverViewQueue(Form form)
        {
            InitializeComponent();
            previousForm = form;
        }

        private void guna2vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        // ✅ Go to Dashboard
        private void DashBtn_Click(object sender, EventArgs e)
        {
            DriverForm dash = new DriverForm(this);
            dash.Show();
            this.Hide();
        }

        // ✅ Go to Settings
        private void SettingsBtn_Click(object sender, EventArgs e)
        {
            DriverSettings settings = new DriverSettings(this);
            settings.Show();
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