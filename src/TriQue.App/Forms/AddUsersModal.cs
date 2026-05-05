using TriQue.Data.Repositories;
using TriQue.DTOs;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


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
            var routes = _repo.GetAllRoutes();
            cboAssignedRoute.DataSource = routes;
            cboAssignedRoute.DisplayMember = "RouteName";
            cboAssignedRoute.ValueMember = "RouteID";

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
            cboAssignedRoute.Visible = isDriver;
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

            int routeID = 0;
            int levelID = 3;

            if (isDriver)
            {
                if (cboAssignedRoute.SelectedItem is not RouteItem ri)
                {
                    MessageBox.Show("Please select a route.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                routeID = ri.RouteID;
            }
            else
            {
                levelID = cboAdminLevel.SelectedIndex + 1;
            }

            try
            {
                var result = _repo.AddUser(fn, ln, phone, roleID, routeID, levelID);

                MessageBox.Show(
                    "User added successfully!\n\n" +
                    $"Username: {result.Username}\n" +
                    $"Temporary Password: {result.TempPassword}\n\n" +
                    "Share these with the user.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
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
