using TriQue;
using TriQue.Data.Repositories;
using TriQue.Forms;
using TriQue.Services;

namespace Trique.Forms
{
    public partial class AdminManageUsers : Form
    {
        private int _userID;
        private readonly UserRepository _repo = new();

        public AdminManageUsers(int userID)
        {
            InitializeComponent();
            _userID = userID;
            SetupGrid();
            LoadUsers();
        }

        private void SetupGrid()
        {
            UserListDataGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            UserListDataGrid.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            UserListDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UserID",
                Visible = false
            });


            UserListDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FullName",
                HeaderText = "Name",
                Width = 180,
                MinimumWidth = 180,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            UserListDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PhoneNumber",
                HeaderText = "Phone #",
                Width = 160,
                MinimumWidth = 160,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            UserListDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RoleName",
                HeaderText = "Role",
                Width = 120,
                MinimumWidth = 120,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });


            UserListDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "AssignedRoute",
                HeaderText = "Assigned Route",
                Width = 200,
                MinimumWidth = 200,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            UserListDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Status",
                HeaderText = "Status",
                Width = 10,
                MinimumWidth = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });


            var editCol = new DataGridViewButtonColumn
            {
                Name = "EditCol",
                HeaderText = "Actions",
                Text = "✏ Edit",
                UseColumnTextForButtonValue = true,
                Width = 100,
                MinimumWidth = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                FlatStyle = FlatStyle.Flat
            };
            editCol.DefaultCellStyle.BackColor = Color.FromArgb(220, 53, 69);
            editCol.DefaultCellStyle.ForeColor = Color.White;
            editCol.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            editCol.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 53, 69);
            editCol.DefaultCellStyle.SelectionForeColor = Color.White;
            editCol.DefaultCellStyle.BackColor = Color.FromArgb(220, 53, 69);
            UserListDataGrid.Columns.Add(editCol);


            var viewCol = new DataGridViewButtonColumn
            {
                Name = "ViewCol",
                HeaderText = "",
                Text = "👁 View",
                UseColumnTextForButtonValue = true,
                Width = 100,
                MinimumWidth = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                FlatStyle = FlatStyle.Flat
            };
            viewCol.DefaultCellStyle.BackColor = Color.FromArgb(13, 110, 253);
            viewCol.DefaultCellStyle.ForeColor = Color.White;
            viewCol.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            viewCol.DefaultCellStyle.SelectionBackColor = Color.FromArgb(13, 110, 253);
            viewCol.DefaultCellStyle.SelectionForeColor = Color.White;
            UserListDataGrid.Columns.Add(viewCol);
        }

        private void LoadUsers(string search = "")
        {
            var users = _repo.GetAllUsers(search);
            UserListDataGrid.Rows.Clear();

            foreach (var u in users)
            {
                int rowIndex = UserListDataGrid.Rows.Add(
                    u.UserID,
                    u.FullName,
                    u.PhoneNumber,
                    u.RoleName,
                    u.AssignedRoute,
                    u.Status
                );

                var row = UserListDataGrid.Rows[rowIndex];

                var statusCell = row.Cells["Status"];
                statusCell.Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                statusCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                statusCell.Style.BackColor = Color.White;
                statusCell.Style.SelectionBackColor = Color.White;

                statusCell.Style.ForeColor = u.Status switch
                {
                    "Waiting" => Color.FromArgb(255, 193, 7),   // yellow
                    "OnTrip" => Color.FromArgb(40, 167, 69),   // green
                    "Finished" => Color.FromArgb(0, 123, 255),   // blue
                    "Active" => Color.FromArgb(40, 167, 69),   // green
                    _ => Color.Gray
                };

                statusCell.Style.SelectionForeColor = statusCell.Style.ForeColor;

                var editCell = row.Cells["EditCol"];
                editCell.Style.BackColor = Color.FromArgb(220, 53, 69);
                editCell.Style.ForeColor = Color.White;
                editCell.Style.SelectionBackColor = Color.FromArgb(220, 53, 69);
                editCell.Style.SelectionForeColor = Color.White;
                editCell.Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                editCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                var viewCell = row.Cells["ViewCol"];
                viewCell.Style.BackColor = Color.FromArgb(13, 110, 253);
                viewCell.Style.ForeColor = Color.White;
                viewCell.Style.SelectionBackColor = Color.FromArgb(13, 110, 253);
                viewCell.Style.SelectionForeColor = Color.White;
                viewCell.Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                viewCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }
        

        private void SearchBar_TextChanged(object sender, EventArgs e)
        {
            LoadUsers(SearchBar.Text.Trim());
        }

        private async void AddUserBtn_Click_1(object sender, EventArgs e)
        {
            var modal = new AddUsersModal();

            await ModalAnimator.ShowModalAsync(this, modal);

            if (modal.DialogResult == DialogResult.OK)
                LoadUsers();
        }

        // action buttons
        private async void UserListDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var rawValue = UserListDataGrid.Rows[e.RowIndex].Cells[0].Value;

            if (rawValue == null || rawValue == DBNull.Value) return;

            int userID = Convert.ToInt32(rawValue);

            string colName = UserListDataGrid.Columns[e.ColumnIndex].Name;

            if (colName == "EditCol")
            {
                var modal = new EditUserInformationModal(userID);

                await ModalAnimator.ShowModalAsync(this, modal);

                if (modal.DialogResult == DialogResult.OK)
                    LoadUsers();
            }
            else if (colName == "ViewCol")
            {
                var modal = new UserDetailsModal(userID);

                await ModalAnimator.ShowModalAsync(this, modal);
            }
        }

        // navbar
        private async void DashboardBtn_Click(object sender, EventArgs e)
        {
            await FormAnimator.SwitchAsync(this, new AdminForm(_userID));
        }

        private async void ViewQueueBtn_Click_1(object sender, EventArgs e)
        {
            await FormAnimator.SwitchAsync(this, new AdminViewQueue(_userID));
        }

        private async void GenerateReportBtn_Click(object sender, EventArgs e)
        {
            var repo = new UserRepository();
            int level = repo.GetAdminLevel(_userID);

            // SuperAdmin and Toda Officer only
            if (level != 1 && level != 2)
            {
                MessageBox.Show(
                    "Access denied. Only SuperAdmins and Toda Officers can manage users.",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            await FormAnimator.SwitchAsync(this, new AdminGenerateReport(_userID));
        }

        private async void SettingsBtn_Click(object sender, EventArgs e)
        {
            await FormAnimator.SwitchAsync(this, new AdminSettings(_userID));
        }

        private async void ManageUsersBtn_Click(object sender, EventArgs e)
        {
            var repo = new UserRepository();
            int level = repo.GetAdminLevel(_userID);

            // SuperAdmin only
            if (level != 1)
            {
                MessageBox.Show(
                    "Access denied. Only SuperAdmins can manage users.",
                    "Access Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            await FormAnimator.SwitchAsync(this, new AdminManageUsers(_userID));
        }

        private async void LogoutBtn_Click(object sender, EventArgs e)
        {
            var authService = new AuthenticationService();
            authService.Logout(_userID);

            await FormAnimator.SwitchAsync(this, new LoginForm());
        }
    }
}