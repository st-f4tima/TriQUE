using TriQue.Data.Repositories;

namespace TriQue.Forms
{
    public partial class SetPasswordModal : Form
    {
        private readonly int _userID;
        private readonly UserRepository _repo = new();

        public SetPasswordModal(int userID)
        {
            InitializeComponent();
            _userID = userID;

            txtNewPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;
        }

        private void chkShowNew_CheckedChanged_1(object sender, EventArgs e)
        {
            txtNewPassword.UseSystemPasswordChar = !chkShowNew.Checked;
        }

        private void chkShowConfirm_CheckedChanged_1(object sender, EventArgs e)
        {
            txtConfirmPassword.UseSystemPasswordChar = !chkShowConfirm.Checked;
        }

        private void ConfirmBtn_Click(object sender, EventArgs e)
        {
            string np = txtNewPassword.Text.Trim();
            string cp = txtConfirmPassword.Text.Trim();

            if (np.Length < 6)
            {
                lblError.Text = "Password must be at least 6 characters.";
                lblError.Visible = true;
                return;
            }
            if (np != cp)
            {
                lblError.Text = "Passwords do not match.";
                lblError.Visible = true;
                return;
            }

            _repo.SetNewPassword(_userID, np);
            MessageBox.Show("Password set successfully!", "Done",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();

        }
    }
}