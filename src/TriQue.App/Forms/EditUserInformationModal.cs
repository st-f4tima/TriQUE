using TriQue.Data.Repositories;
using TriQue.Models;

namespace TriQue
{
    public partial class EditUserInformationModal : Form
    {
        private readonly int _userID;
        private readonly UserRepository _repo = new();
        public EditUserInformationModal(int userID)
        {
            InitializeComponent();
            _userID = userID;
            LoadRoutes();
            cboRole.SelectedIndexChanged += CboRole_Changed;
            LoadData();
        }

        private void LoadRoutes()
        {
            cboRole.Items.Clear();
            cboRole.Items.AddRange(new[] { "Driver", "Admin" });

            cboAdminLevel.Items.Clear();
            cboAdminLevel.Items.AddRange(new[] { "SuperAdmin", "TodaOfficer", "Staff" });

            var driverRepo = new DriverRepository();
            var groups = driverRepo.GetAllGroups();
            cboAssignedRoute.DataSource = groups;
            cboAssignedRoute.DisplayMember = "GroupName";
            cboAssignedRoute.ValueMember = "GroupID";
        }

        private void LoadData()
        {
            var d = _repo.GetUserDetail(_userID);
            if (d == null) return;

            txtFullName.Text = d.FullName;
            txtPhoneNumber.Text = d.PhoneNumber;
            cboRole.SelectedIndex = d.RoleID == 2 ? 1 : 0;

            CboRole_Changed(null, EventArgs.Empty); 

            cboAssignedRoute.SelectedValue = d.GroupID; 

            if (d.RoleID == 2)
            {
                int adminLevel = _repo.GetAdminLevel(_userID);
                cboAdminLevel.SelectedIndex = adminLevel - 1;
            }
        }

        private void CboRole_Changed(object? sender, EventArgs e)
        {
            bool isDriver = cboRole.SelectedIndex == 0;
            lblAssignedRoute.Visible = isDriver;
            cboAssignedRoute.Visible = isDriver;
            lblAdminLevel.Visible = !isDriver;
            cboAdminLevel.Visible = !isDriver;
        }

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
                _repo.UpdateUser(_userID, name, phone, roleID, groupID, levelID);
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
                _repo.DeleteUser(_userID);
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

        private void txtPhoneNumber_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
