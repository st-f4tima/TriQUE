using TriQue.Enums;
using TriQue.Helpers;
using TriQue.Helpers.Animation;
using TriQue.Models;
using TriQue.Services;

namespace TriQue.Forms
{
    public partial class LoginForm : Form
    {
        private readonly AuthenticationService _authService = new AuthenticationService();

        private Label lockLabel;
        private System.Windows.Forms.Timer _lockTimer;

        public LoginForm()
        {
            InitializeComponent();
            InitializeLockLabel();
            InitializePasswordField();
            ApplyFonts();
        }

        private void ApplyFonts()
        {
            this.Font = FontHelper.RobotoRegular;
            lblGreeting.Font = FontHelper.GetRoboto(24f, FontStyle.Bold);
            lblLoginDesc.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblPass.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblUsername.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            checkBox1.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            tbUsername.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            tbPassword1.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);

        }

        private void InitializeLockLabel()
        {
            lockLabel = new Label
            {
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Location = new Point(412, 534),
                Visible = false
            };
            this.Controls.Add(lockLabel);
            lockLabel.BringToFront();
        }


        private void InitializePasswordField()
        {
            tbPassword1.UseSystemPasswordChar = true;
        }

        private async void LoginBtn_Click_1(object sender, EventArgs e)
        {
            string username = tbUsername.Text.Trim();
            string password = tbPassword1.Text.Trim();

            if (!_authService.Login(username, password, out string message))
            {
                HandleFailure(username, message);
                return;
            }

            var user = _authService.GetCurrentUser();

            if (_authService.CurrentUserNeedsPasswordReset())
            {
                using var setPwdForm = new SetPasswordModal(user.UserID);
                if (setPwdForm.ShowDialog() != DialogResult.OK) return;
            }

            ShowSuccess(message);

            await FormAnimator.SwitchAsync(this, GetTargetView(user));
        }

        private void HandleFailure(string username, string message)
        {
            int secondsLeft = _authService.GetLockSecondsRemaining(username);

            if (message.Contains("locked") || secondsLeft > 0)
            {
                ShowWarning(message);
                if (secondsLeft > 0) StartLockUI(secondsLeft);
            }
            else
            {
                ShowError(message);
            }
        }

        private Form GetTargetView(User user)
        {
            return user.Role switch
            {
                UserRole.Admin => new AdminForm(user.UserID),
                UserRole.Driver => new DriverForm(user.UserID),

                _ => throw new Exception(
                    $"No dashboard defined for role: {user.Role}")
            };
        }

        // ui countdown
        private void StartLockUI(int seconds)
        {
            _lockTimer?.Stop();
            _lockTimer?.Dispose();

            int remaining = seconds;
            LoginBtn.Enabled = false;
            lockLabel.Visible = true;

            _lockTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _lockTimer.Tick += (s, ev) =>
            {
                remaining--;
                lockLabel.Text = $"Account locked — {remaining}s remaining";

                if (remaining <= 0)
                {
                    _lockTimer.Stop();
                    LoginBtn.Enabled = true;
                    lockLabel.Visible = false;
                }
            };
            _lockTimer.Start();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            tbPassword1.UseSystemPasswordChar = !checkBox1.Checked;
        }

        #region Notifications
        private void ShowError(string msg) => MessageBox.Show(msg, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        private void ShowWarning(string msg) => MessageBox.Show(msg, "Account Locked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        private void ShowSuccess(string msg) => MessageBox.Show(msg, "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
        #endregion

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void tbPassword1_TextChanged(object sender, EventArgs e)
        {

        }


    }
}