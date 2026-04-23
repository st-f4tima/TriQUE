using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

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

            // 🔴 lock label
            lockLabel = new Label();
            lockLabel.AutoSize = true;
            lockLabel.ForeColor = Color.Red;
            // Ini-akyat ko ng konti yung text (from 450 to 420) para sakto sa pwesto ng nawalang button
            lockLabel.Location = new Point(55, 420);
            panel1.Controls.Add(lockLabel);

            lockLabel.BringToFront();

            // ⏳ timer
            lockTimer = new System.Windows.Forms.Timer();
            lockTimer.Interval = 1000;
            lockTimer.Tick += LockTimer_Tick;
        }

        private string GetConnectionString()
        {
            var conn = AppConfig.Configuration.GetConnectionString("Default");
            string fullPath = Path.GetFullPath(conn, AppContext.BaseDirectory);
            return $"Data Source={fullPath}";
        }

        // =========================
        // LOGIN BUTTON
        // =========================
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBoxPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Invalid username or password.");
                return;
            }

            if (lockTimer.Enabled)
            {
                MessageBox.Show(
                    "Login is temporarily locked. Please try again later.",
                    "Account Locked",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
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
                    // ❌ WRONG USERNAME
                    if (!reader.Read())
                    {
                        HandleFailedLogin(conn, null);
                        ShowError("Invalid username or password.");
                        return;
                    }

                    int userId = reader.GetInt32(0);
                    int roleId = reader.GetInt32(1);

                    string dbPassword = reader.GetString(2);

                    // ❌ WRONG PASSWORD (Hardcoded to 1234)
                    if (password != "1234")
                    {
                        HandleFailedLogin(conn, userId);
                        ShowError("Invalid username or password.");
                        return;
                    }

                    // ✅ SUCCESS
                    ResetAttempts(conn, userId);

                    string role = roleId == 2 ? "Admin" : "Driver";

                    MessageBox.Show(
                        $"{role} successful login!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    failedAttempts = 0;
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
        // ERROR MESSAGE (RED X STYLE)
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

        // =========================
        // LOCK SYSTEM
        // =========================
        private void StartLock()
        {
            lockSeconds = 60;
            lockLabel.Text = "Too many attempts. Try again in 60 seconds.";

            // 1. ITAGO YUNG LOGIN BUTTON MUNA
            guna2Button1.Visible = false;

            // 2. I-UPDATE ANG SCREEN PARA MAWALA AGAD YUNG BUTTON
            this.Refresh();

            // 3. SIMULAN ANG TIMER HABANG NAKABUKAS ANG POPUP
            lockTimer.Start();

            // 4. SAKA LALABAS YUNG MESSAGE BOX
            MessageBox.Show(
                "Login is temporarily locked. Please try again later.",
                "Account Locked",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }

        private void LockTimer_Tick(object sender, EventArgs e)
        {
            lockSeconds--;

            lockLabel.Text = $"Locked: {lockSeconds} seconds remaining";

            if (lockSeconds <= 0)
            {
                lockTimer.Stop();

                // IBABALIK YUNG LOGIN BUTTON KAPAG TAPOS NA
                guna2Button1.Visible = true;

                lockLabel.Text = "";
                failedAttempts = 0;
            }
        }

        // =========================
        // UI EVENTS (unchanged)
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
    }
}