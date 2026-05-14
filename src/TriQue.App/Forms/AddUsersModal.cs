using TriQue.Data.Repositories;
using TriQue.DTOs;
using TriQue.Models;

namespace TriQue
{
    public partial class AddUsersModal : Form
    {
        private readonly UserRepository _repo = new();
        public string GeneratedPassword { get; private set; } = "";
        public AddUsersModal()
        {
            InitializeComponent();
            LoadRoutes();
            cboRole.SelectedIndexChanged += CboRole_Changed;
            cboRole.SelectedIndex = 0;
        }

        private void LoadRoutes()
        {
            var driverRepo = new DriverRepository();
            var groups = driverRepo.GetAllGroups();
            cboAssignedGroup.DataSource = groups;
            cboAssignedGroup.DisplayMember = "GroupName";
            cboAssignedGroup.ValueMember = "GroupID";

            cboRole.Items.Clear();
            cboRole.Items.AddRange(new[] { "Driver", "Admin" });

            cboAdminLevel.Items.Clear();
            cboAdminLevel.Items.AddRange(new[] { "SuperAdmin", "TodaOfficer", "Staff" });
            cboAdminLevel.SelectedIndex = 2;
        }

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
            string fn = txtFirstName.Text.Trim();
            string ln = txtLastName.Text.Trim();
            string phone = txtPhoneNumber.Text.Trim();
            bool isDriver = cboRole.SelectedIndex == 0;
            int roleID = isDriver ? 1 : 2;

            if (string.IsNullOrWhiteSpace(fn) || string.IsNullOrWhiteSpace(ln) ||
                string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Please fill in all fields.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int groupID = 0;
            int levelID = 3;

            if (isDriver)
            {
                if (cboAssignedGroup.SelectedItem is not DriverGroup dg)
                {
                    MessageBox.Show("Please select a group.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                groupID = dg.GroupID; 
            }
            else
            {
                levelID = cboAdminLevel.SelectedIndex + 1;
            }

            try
            {
                var result = _repo.AddUser(fn, ln, phone, roleID, groupID, levelID);

                MessageBox.Show(
                    "User added successfully!\n\n" +
                    $"Username: {result.Username}\n" +
                    $"Temporary Password: {result.TempPassword}\n\n" +
                    "Share these with the user.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
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
}
