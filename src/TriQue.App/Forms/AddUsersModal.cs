using TriQue.Models;
using TriQue.Services;

namespace TriQue
{
    public partial class AddUsersModal : Form
    {
        private readonly UserService _userService = new();
        private readonly DriverService _driverService = new();

        public AddUsersModal()
        {
            InitializeComponent();

            cboRole.SelectedIndexChanged += CboRole_Changed;

            LoadFormDefaults();
        }

        #region Load Methods

        private void LoadFormDefaults()
        {
            LoadRoleOptions();
            LoadAdminLevels();
            LoadDriverGroups();

            cboRole.SelectedIndex = 0;
        }

        private void LoadRoleOptions()
        {
            cboRole.Items.Clear();
            cboRole.Items.AddRange(new[] { "Driver", "Admin" });
        }

        private void LoadAdminLevels()
        {
            cboAdminLevel.Items.Clear();
            cboAdminLevel.Items.AddRange(new[] { "SuperAdmin", "TodaOfficer", "Staff" });
            cboAdminLevel.SelectedIndex = 2;
        }

        private void LoadDriverGroups()
        {
            var groups = _driverService.GetAllGroups();

            cboAssignedGroup.DataSource = groups;
            cboAssignedGroup.DisplayMember = "GroupName";
            cboAssignedGroup.ValueMember = "GroupID";
        }

        #endregion

        #region Events

        private void CboRole_Changed(object? sender, EventArgs e)
        {
            bool isDriver = cboRole.SelectedIndex == 0;

            lblAssignedRoute.Visible = isDriver;
            cboAssignedGroup.Visible = isDriver;

            lblAdminLevel.Visible = !isDriver;
            cboAdminLevel.Visible = !isDriver;
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string phoneNumber = txtPhoneNumber.Text.Trim();

            bool isDriver = cboRole.SelectedIndex == 0;
            int roleID = isDriver ? 1 : 2;

            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(phoneNumber))
            {
                MessageBox.Show(
                    "Please fill in all fields.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            int groupID = 0;
            int adminLevelID = 3;

            if (isDriver)
            {
                if (cboAssignedGroup.SelectedItem is not DriverGroup group)
                {
                    MessageBox.Show(
                        "Please select a group.",
                        "Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                groupID = group.GroupID;
            }
            else
            {
                adminLevelID = cboAdminLevel.SelectedIndex + 1;
            }

            try
            {
                var result = _userService.AddUser(firstName, lastName, phoneNumber, roleID, groupID, adminLevelID);

                MessageBox.Show(
                    "User added successfully!\n\n" +
                    $"Username: {result.Username}\n" +
                    $"Temporary Password: {result.TempPassword}\n\n" +
                    "Share these with the user.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
