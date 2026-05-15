using TriQue.Helpers;
using TriQue.Services;

namespace TriQue.Forms
{
    public partial class SetPasswordModal : Form
    {
        private readonly UserService _userService = new();
        private readonly int _userID;
        public SetPasswordModal(int userID)
        {
            InitializeComponent();
            ApplyFonts();
            
            _userID = userID;

            InitializePasswordFields();
        }

        private void InitializePasswordFields()
        {
            txtNewPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;
        }

        #region Toggle Visibility

        private void chkShowNew_CheckedChanged_1(object sender, EventArgs e)
        {
            txtNewPassword.UseSystemPasswordChar = !chkShowNew.Checked;
        }

        private void chkShowConfirm_CheckedChanged_1(object sender, EventArgs e)
        {
            txtConfirmPassword.UseSystemPasswordChar = !chkShowConfirm.Checked;
        }

        #endregion

        private void ApplyFonts()
        {
            this.Font = FontHelper.RobotoRegular;

            lblNewPassword.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblConfirmPassword.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);

            txtNewPassword.Font = FontHelper.GetRoboto(10, FontStyle.Bold);
            txtConfirmPassword.Font = FontHelper.GetRoboto(10, FontStyle.Bold);

            chkShowNew.Font = FontHelper.GetRoboto(8f, FontStyle.Bold);
            chkShowConfirm.Font = FontHelper.GetRoboto(8f, FontStyle.Bold);

            lblError.Font = FontHelper.GetRoboto(9f, FontStyle.Bold);

            ConfirmBtn.Font = FontHelper.GetRoboto(11f, FontStyle.Bold);
        }

        #region Actions

        private void ConfirmBtn_Click(object sender, EventArgs e)
        {
            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (!IsPasswordValid(newPassword, confirmPassword))
                return;

            _userService.SetNewPassword(_userID, newPassword);

            MessageBox.Show(
                "Password set successfully!",
                "Done",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );

            DialogResult = DialogResult.OK;
            Close();
        }

        #endregion

        #region Validation
        private bool IsPasswordValid(string newPassword, string confirmPassword)
        {
            if (newPassword.Length < 6)
            {
                ShowError("Password must be at least 6 characters.");
                return false;
            }

            if (newPassword != confirmPassword)
            {
                ShowError("Passwords do not match.");
                return false;
            }

            lblError.Visible = false;
            return true;
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }

        #endregion
    }
}