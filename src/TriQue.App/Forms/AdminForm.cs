using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Guna.Charts.WinForms;
using TriQue.Forms;

namespace Trique.Forms
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
            this.Load += AdminForm_Load;
        }

        private void gunaChart1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void AdminForm_Load(object? sender, EventArgs e)
        {
            // ===== PIE CHART =====
            guna2Chart1.Datasets.Clear();

            var pieDataset = new GunaPieDataset();

            pieDataset.DataPoints.Add("On Trip", 58);
            pieDataset.DataPoints.Add("Finished", 26);
            pieDataset.DataPoints.Add("Waiting", 16);

            // colors (THIS WORKS IN YOUR VERSION)
            pieDataset.FillColors.Add(Color.Blue);
            pieDataset.FillColors.Add(Color.Green);
            pieDataset.FillColors.Add(Color.Orange);

            guna2Chart1.Datasets.Add(pieDataset);

            guna2Chart1.XAxes.Display = false;
            guna2Chart1.YAxes.Display = false;
            guna2Chart1.Update();


            // ===== BAR CHART =====
            guna2Chart2.Datasets.Clear();

            var barDataset = new GunaBarDataset();

            barDataset.DataPoints.Add("Route A", 80);
            barDataset.DataPoints.Add("Route B", 60);
            barDataset.DataPoints.Add("Route C", 45);
            barDataset.DataPoints.Add("Route D", 30);
            barDataset.DataPoints.Add("Route E", 20);
            barDataset.DataPoints.Add("Route F", 40);

            // ONE COLOR (safe for your version)
            barDataset.FillColors.Add(Color.Blue);

            guna2Chart2.Datasets.Add(barDataset);

            guna2Chart2.XAxes.GridLines.Display = false;
            guna2Chart2.YAxes.GridLines.Display = false;
            guna2Chart2.Legend.Display = false;

            guna2Chart2.Update();
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            LoginForm adminForm = new LoginForm();
            adminForm.Show();
            this.Hide();
        }

        private void DashboardBtn_Click(object sender, EventArgs e)
        {

        }

        private void ViewQueue_Click(object sender, EventArgs e)
        {
            AdminViewQueue adminForm = new AdminViewQueue();
            adminForm.Show();
            this.Hide();
        }


        private void SettingsBtn_Click(object sender, EventArgs e)
        {
            AdminSettings adminForm = new AdminSettings();
            adminForm.Show();
            this.Hide();
        }

        private void ManageUsersBtn_Click(object sender, EventArgs e)
        {
            AdminManageUsers adminForm = new AdminManageUsers();
            adminForm.Show();
            this.Hide();
        }
    }
}
