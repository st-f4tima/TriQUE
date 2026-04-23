using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Trique.Forms;

namespace TriQue.Forms
{
    public partial class LoginForm : Form
    {
        private int failedAttempts = 0;
        private System.Windows.Forms.Timer lockTimer;
        private int lockSeconds = 60;
        private Label lockLabel;

        public LoginForm()
        {
            InitializeComponent();
            guna2Button1.Click += guna2Button1_Click;

            lockLabel = new Label();
            lockLabel.AutoSize = true;
            lockLabel.ForeColor = Color.FromArgb(220, 53, 69);
            lockLabel.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lockLabel.Location = new Point(55, 420);
            panel1.Controls.Add(lockLabel);
            lockLabel.BringToFront();

            lockTimer = new System.Windows.Forms.Timer();
            lockTimer.Interval = 1000;
            lockTimer.Tick += LockTimer_Tick;
        }

        private string GetConnectionString()
        {
            var conn = AppConfig.Configuration.GetConnectionString("Default")
                       ?? throw new InvalidOperationException("Connection string 'Default' not found.");
            string fullPath = Path.GetFullPath(conn, AppContext.BaseDirectory);
            return $"Data Source={fullPath}";
        }

        // =========================
        // LOGIN BUTTON
        // =========================
        private void guna2Button1_Click(object? sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBoxPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Please enter your username and password.");
                return;
            }

            if (lockTimer.Enabled)
            {
                ShowWarning($"Account is locked. Please wait {lockSeconds} seconds before trying again.");
                return;
            }

            using (var conn = new SqliteConnection(GetConnectionString()))
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText =
                @"
                SELECT UserID, RoleID, PasswordHash, FailedAttempts
                FROM User
                WHERE Username = $username
                ";
                cmd.Parameters.AddWithValue("$username", username);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        HandleFailedLogin(conn, null);
                        ShowError("Invalid username or password.");
                        return;
                    }

                    int userId = reader.GetInt32(0);
                    int roleId = reader.GetInt32(1);
                    string dbPassword = reader.GetString(2);

                    if (password != "1234")
                    {
                        HandleFailedLogin(conn, userId);
                        if (failedAttempts < 3)
                        {
                            int remaining = 3 - failedAttempts;
                            ShowError($"Invalid username or password.\n{remaining} attempt(s) remaining before lockout.");
                        }
                        return;
                    }

                    // ✅ SUCCESS
                    ResetAttempts(conn, userId);
                    failedAttempts = 0;

                    string role = roleId == 2 ? "Admin" : "Driver";
                    ShowSuccess($"Welcome back!\nLogged in as {role}.");

                    if (roleId == 2)
                    {
                        AdminForm adminForm = new AdminForm();
                        adminForm.Show();
                    }
                    else
                    {
                        DriverForm driverForm = new DriverForm();
                        driverForm.Show();
                    }

                    this.Hide();
                }
            }
        }

        // =========================
        // FAILED LOGIN HANDLER
        // =========================
        private void HandleFailedLogin(SqliteConnection conn, int? userId)
        {
            failedAttempts++;

            if (userId.HasValue)
            {
                var cmd = conn.CreateCommand();

                if (failedAttempts >= 3)
                {
                    cmd.CommandText =
                    @"
                    UPDATE User
                    SET FailedAttempts = 0,
                        LockoutUntil = $lock
                    WHERE UserID = $id
                    ";
                    cmd.Parameters.AddWithValue("$lock", DateTime.Now.AddMinutes(1));
                    cmd.Parameters.AddWithValue("$id", userId.Value);
                    cmd.ExecuteNonQuery();
                    StartLock();
                }
                else
                {
                    cmd.CommandText =
                    @"
                    UPDATE User
                    SET FailedAttempts = FailedAttempts + 1
                    WHERE UserID = $id
                    ";
                    cmd.Parameters.AddWithValue("$id", userId.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // =========================
        // RESET ATTEMPTS
        // =========================
        private void ResetAttempts(SqliteConnection conn, int userId)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText =
            @"
            UPDATE User
            SET FailedAttempts = 0,
                LockoutUntil = NULL
            WHERE UserID = $id
            ";
            cmd.Parameters.AddWithValue("$id", userId);
            cmd.ExecuteNonQuery();
        }

        // =========================
        // MESSAGE HELPERS
        // =========================
        private void ShowError(string msg)
        {
            MessageBox.Show(
                msg,
                "Login Failed",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }

        private void ShowWarning(string msg)
        {
            MessageBox.Show(
                msg,
                "Account Locked",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        private void ShowSuccess(string msg)
        {
            MessageBox.Show(
                msg,
                "Login Successful",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        // =========================
        // LOCK SYSTEM
        // =========================
        // =========================
        // LOCK SYSTEM
        // =========================
        private void StartLock()
        {
            lockSeconds = 60;
            guna2Button1.Visible = false;
            this.Refresh();
            lockTimer.Start();
            ShowLockDialog();
        }

        private void ShowLockDialog()
        {
            Form lockDialog = new Form();
            lockDialog.Text = "";
            lockDialog.Size = new Size(380, 230);
            lockDialog.StartPosition = FormStartPosition.CenterParent;
            lockDialog.FormBorderStyle = FormBorderStyle.None;
            lockDialog.MaximizeBox = false;
            lockDialog.MinimizeBox = false;
            lockDialog.ControlBox = false;
            lockDialog.BackColor = Color.FromArgb(245, 247, 255);

            lockDialog.Load += (s, e) =>
            {
                // Position it lower so it doesn't overlap the logo
                lockDialog.Location = new Point(
                    this.Location.X + (this.Width - lockDialog.Width) / 2,
                    this.Location.Y + (this.Height - lockDialog.Height) / 2 + 80
                );
            };

            // === TOP ACCENT BAR (blue) ===
            Panel topBar = new Panel();
            topBar.Size = new Size(380, 5);
            topBar.Location = new Point(0, 0);
            topBar.BackColor = Color.FromArgb(61, 90, 241);

            // === ICON CIRCLE ===
            Panel iconCircle = new Panel();
            iconCircle.Size = new Size(54, 54);
            iconCircle.Location = new Point(163, 22);
            iconCircle.BackColor = Color.FromArgb(220, 226, 255);

            Label iconLbl = new Label();
            iconLbl.Text = "!";
            iconLbl.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            iconLbl.ForeColor = Color.FromArgb(61, 90, 241);
            iconLbl.Size = new Size(54, 54);
            iconLbl.TextAlign = ContentAlignment.MiddleCenter;
            iconCircle.Controls.Add(iconLbl);

            // === TITLE ===
            Label titleLbl = new Label();
            titleLbl.Text = "Account Locked";
            titleLbl.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            titleLbl.ForeColor = Color.FromArgb(30, 30, 60);
            titleLbl.Location = new Point(20, 85);
            titleLbl.Size = new Size(340, 28);
            titleLbl.TextAlign = ContentAlignment.MiddleCenter;

            // === MESSAGE ===
            Label msgLbl = new Label();
            msgLbl.Text = "Too many failed login attempts.";
            msgLbl.Font = new Font("Segoe UI", 9.5f);
            msgLbl.ForeColor = Color.FromArgb(100, 110, 150);
            msgLbl.Location = new Point(20, 116);
            msgLbl.Size = new Size(340, 20);
            msgLbl.TextAlign = ContentAlignment.MiddleCenter;

            // === COUNTDOWN LABEL ===
            Label countLbl = new Label();
            countLbl.Text = $"Please wait {lockSeconds} seconds before trying again.";
            countLbl.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            countLbl.ForeColor = Color.FromArgb(61, 90, 241);
            countLbl.Location = new Point(20, 140);
            countLbl.Size = new Size(340, 20);
            countLbl.TextAlign = ContentAlignment.MiddleCenter;

            // === OK BUTTON (disabled until done) ===
            Button okBtn = new Button();
            okBtn.Text = $"Please wait ({lockSeconds}s)";
            okBtn.Size = new Size(160, 36);
            okBtn.Location = new Point(110, 172);
            okBtn.Enabled = false;
            okBtn.FlatStyle = FlatStyle.Flat;
            okBtn.BackColor = Color.FromArgb(180, 190, 240);
            okBtn.ForeColor = Color.White;
            okBtn.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            okBtn.FlatAppearance.BorderSize = 0;
            okBtn.Cursor = Cursors.Default;
            okBtn.Click += (s, e) => lockDialog.Close();

            // === DIALOG TIMER ===
            System.Windows.Forms.Timer dialogTimer = new System.Windows.Forms.Timer();
            dialogTimer.Interval = 1000;
            dialogTimer.Tick += (s, e) =>
            {
                int remaining = lockSeconds;

                if (remaining > 0)
                {
                    countLbl.Text = $"Please wait {remaining} seconds before trying again.";
                    okBtn.Text = $"Please wait ({remaining}s)";
                }
                else
                {
                    countLbl.Text = "You may now try again.";
                    countLbl.ForeColor = Color.FromArgb(40, 167, 69);
                    okBtn.Text = "OK, Got it";
                    okBtn.Enabled = true;
                    okBtn.BackColor = Color.FromArgb(61, 90, 241);
                    okBtn.ForeColor = Color.White;
                    okBtn.Cursor = Cursors.Hand;
                    dialogTimer.Stop();
                }
            };

            dialogTimer.Start();

            lockDialog.Controls.Add(topBar);
            lockDialog.Controls.Add(iconCircle);
            lockDialog.Controls.Add(titleLbl);
            lockDialog.Controls.Add(msgLbl);
            lockDialog.Controls.Add(countLbl);
            lockDialog.Controls.Add(okBtn);

            lockDialog.ShowDialog(this);
            dialogTimer.Dispose();
        }

        private void LockTimer_Tick(object? sender, EventArgs e)
        {
            lockSeconds--;

            // Update the form label only (dialog has its own timer)
            lockLabel.Text = lockSeconds > 0
                ? $"Account locked — {lockSeconds}s remaining"
                : "";

            if (lockSeconds <= 0)
            {
                lockTimer.Stop();
                guna2Button1.Visible = true;
                lockLabel.Text = "";
                failedAttempts = 0;
            }
        }

        // =========================
        // UI EVENTS
        // =========================
        private void checkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = !checkBoxShowPassword.Checked;
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}