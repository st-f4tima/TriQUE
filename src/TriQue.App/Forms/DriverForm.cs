using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TriQue.Forms
{
    public partial class DriverForm : Form
    {
        public DriverForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await webView21.EnsureCoreWebView2Async();

            string html = @"
    <html>
    <body style='margin:0'>
        <iframe 
            src='https://www.google.com/maps/embed?pb=!1m10!1m8!1m3!1d3874.8815970302435!2d121.070066!3d13.7860105!3m2!1i1024!2i768!4f13.1!5e0!3m2!1sen!2sph!4v1776747251447!5m2!1sen!2sph'
            width='100%' 
            height='100%' 
            style='border:0;' 
            allowfullscreen='' 
            loading='lazy'>
        </iframe>
    </body>
    </html>";

            webView21.NavigateToString(html);
        }

        private void guna2Panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void webView21_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }



        private void guna2ImageButton5_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }


        private Form DriverViewQueue;

        public DriverForm(Form form)
        {
            InitializeComponent();
            DriverViewQueue = form;
        }

        private void DashBtn_Click(object sender, EventArgs e)
        {

        }

        private void ViewQueueBtn_Click(object sender, EventArgs e)
        {
            DriverViewQueue viewQueue = new DriverViewQueue(this);
            viewQueue.Show();
            this.Hide();
        }

        private void SettinsBtn_Click(object sender, EventArgs e)
        {
            DriverSettings settings = new DriverSettings(this);
            settings.Show();
            this.Hide();
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Close();
        }
    }
}