using TriQue.Models;
using TriQue.Services;

namespace TriQue
{
    public partial class EditUserInformationModal : Form
    {
        private readonly UserService _userService = new();
        private readonly DriverService _driverService = new();

        private readonly int _userID;

        public EditUserInformationModal(int userID)
        {
            InitializeComponent();

            _userID = userID;

            cboRole.SelectedIndexChanged += CboRole_Changed;

            LoadRoleOptions();
            LoadUserData();
        }

        #region Load Methods

        private void LoadRoleOptions()
        {
            LoadRoles();
            LoadAdminLevels();
            LoadDriverGroups();
        }

        private void LoadRoles()
        {
            cboRole.Items.Clear();
            cboRole.Items.AddRange(new[] { "Driver", "Admin" });
        }

        private void LoadAdminLevels()
        {
            cboAdminLevel.Items.Clear();
            cboAdminLevel.Items.AddRange(new[] { "SuperAdmin", "TodaOfficer", "Staff" });
        }

        private void LoadDriverGroups()
        {
            var groups = _driverService.GetAllGroups();

            cboAssignedRoute.DataSource = groups;
            cboAssignedRoute.DisplayMember = "GroupName";
            cboAssignedRoute.ValueMember = "GroupID";
        }

        private void LoadUserData()
        {
            var user = _userService.GetUserDetail(_userID);
            
            if (user == null)
            {
                return;
            }

            txtFullName.Text = user.FullName;
            txtPhoneNumber.Text = user.PhoneNumber;
            cboRole.SelectedIndex = user.RoleID == 2 ? 1 : 0;

            CboRole_Changed(null, EventArgs.Empty); 

            cboAssignedRoute.SelectedValue = user.GroupID; 

            if (user.RoleID == 2)
            {
                int adminLevel = _userService.GetAdminLevel(_userID);
                cboAdminLevel.SelectedIndex = adminLevel - 1;
            }
        }

        #endregion

        private void CboRole_Changed(object? sender, EventArgs e)
        {
            bool isDriver = cboRole.SelectedIndex == 0;
            lblAssignedRoute.Visible = isDriver;
            cboAssignedRoute.Visible = isDriver;
            lblAdminLevel.Visible = !isDriver;
            cboAdminLevel.Visible = !isDriver;
        }

        #region actions
        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            string name = txtFullName.Text.Trim();
            string phone = txtPhoneNumber.Text.Trim();
            int roleID = cboRole.SelectedIndex == 0 ? 1 : 2;
            int groupID = cboAssignedRoute.SelectedItem is DriverGroup g ? g.GroupID : 0; 
            int levelID = cboAdminLevel.SelectedIndex + 1; 


            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Please fill in all fields.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _userService.UpdateUser(_userID, name, phone, roleID, groupID, levelID);
                MessageBox.Show("User updated successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to delete this user?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _userService.DeleteUser(_userID);
                MessageBox.Show("User deleted.", "Deleted",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
        #endregion
}
