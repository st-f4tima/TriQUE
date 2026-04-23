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
    public partial class LoginForm : Form
    {
        private AuthenticationService _auth = new AuthenticationService();
        private Label lockLabel;
        private System.Windows.Forms.Timer _lockTimer;

        public LoginForm()
        {
            InitializeComponent();
            guna2Button1.Click += guna2Button1_Click;

            lockLabel = new Label();
            lockLabel.AutoSize = true;
            lockLabel.ForeColor = Color.FromArgb(220, 53, 69);
            lockLabel.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lockLabel.Location = new Point(55, 450);
            lockLabel.Visible = false;
            lockLabel.Text = "";
            lockLabel.Size = new Size(0, 0);
            lockLabel.MinimumSize = new Size(0, 0);
            panel1.Controls.Add(lockLabel);
            lockLabel.BringToFront();
        }

        // login button
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBoxPassword.Text.Trim();

            bool success = _auth.Login(username, password, out string message);

            if (!success)
            {
                int secondsLeft = _auth.GetLockSecondsRemaining(username);
                if (message.Contains("locked"))
                {
                    ShowWarning(message);
                }
                else
                {
                    ShowError(message);
                }

                if (secondsLeft > 0)
                {
                    StartLockUI(secondsLeft);
                }

                return;
            }

            ShowSuccess(message);

            var user = _auth.GetCurrentUser();

            Form nextForm = user is Admin
                ? new AdminForm()
                : new DriverForm();

            nextForm.Show();
            this.Hide();
        }

        private void checkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = !checkBoxShowPassword.Checked;
        }

        private void StartLockUI(int seconds)
        {
            if (_lockTimer != null)
            {
                _lockTimer.Stop();
                _lockTimer.Dispose();
            }

            int remaining = seconds;
            guna2Button1.Enabled = false;
            lockLabel.Visible = true;
            lockLabel.Text = $"Account locked — {remaining}s remaining";

            _lockTimer = new System.Windows.Forms.Timer();
            _lockTimer.Interval = 1000;

            _lockTimer.Tick += (s, ev) =>
            {
                remaining--;
                lockLabel.Text = $"Account locked — {remaining}s remaining";

                if (remaining <= 0)
                {
                    _lockTimer.Stop();
                    _lockTimer.Dispose();
                    guna2Button1.Enabled = true;
                    lockLabel.Visible = false;
                    lockLabel.Text = "";
                }
            };

            _lockTimer.Start();
        }

        private void ShowError(string msg)
        {
            MessageBox.Show(msg, "Login Failed",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowWarning(string msg)
        {
            MessageBox.Show(msg, "Account Locked",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ShowSuccess(string msg)
        {
            MessageBox.Show(msg, "Login Successful",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}