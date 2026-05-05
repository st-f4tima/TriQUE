using TriQue;
using TriQue.Data.Repositories;
using TriQue.Forms;

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
                Width = 200,
                MinimumWidth = 200,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            UserListDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PhoneNumber",
                HeaderText = "Phone #",
                Width = 140,
                MinimumWidth = 140,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            UserListDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RoleName",
                HeaderText = "Role",
                Width = 100,
                MinimumWidth = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });


            UserListDataGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "AssignedRoute",
                HeaderText = "Assigned Route",
                Width = 220,
                MinimumWidth = 220,
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
                statusCell.Style.ForeColor = Color.White;
                statusCell.Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                statusCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                statusCell.Style.BackColor = u.Status switch
                {
                    "Active" => Color.FromArgb(0, 200, 83),
                    "OnTrip" => Color.FromArgb(0, 200, 83),
                    "Waiting" => Color.FromArgb(255, 183, 0),
                    "Finished" => Color.FromArgb(108, 117, 125),
                    _ => Color.FromArgb(0, 200, 83)
                };

                statusCell.Style.SelectionBackColor = statusCell.Style.BackColor;
                statusCell.Style.SelectionForeColor = Color.White;

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

        private void AddUserBtn_Click_1(object sender, EventArgs e)
        {
            var modal = new AddUsersModal();
            if (modal.ShowDialog() == DialogResult.OK)
                LoadUsers();
        }

        // action buttons
        private void UserListDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var rawValue = UserListDataGrid.Rows[e.RowIndex].Cells[0].Value;

            if (rawValue == null || rawValue == DBNull.Value) return;

            int userID = Convert.ToInt32(rawValue);

            string colName = UserListDataGrid.Columns[e.ColumnIndex].Name;

            if (colName == "EditCol")
            {
                var modal = new EditUserInformationModal(userID);
                if (modal.ShowDialog() == DialogResult.OK)
                    LoadUsers();
            }
            else if (colName == "ViewCol")
            {
                var modal = new UserDetailsModal(userID);
                modal.ShowDialog();
            }
        }

        // navbar
        private void DashboardBtn_Click(object sender, EventArgs e)
        {
            new AdminForm(_userID).Show();
            this.Hide();
        }

        private void ViewQueueBtn_Click_1(object sender, EventArgs e)
        {
            new AdminViewQueue(_userID).Show();
            this.Hide();
        }

        private void GenerateReportBtn_Click_1(object sender, EventArgs e)
        {
            new AdminGenerateReport(_userID).Show();
            this.Hide();
        }

        private void SettingsBtn_Click(object sender, EventArgs e)
        {
            new AdminSettings(_userID).Show();
            this.Hide();
        }

        private void ManageUsersBtn_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void LogoutBtn_Click(object sender, EventArgs e)
        {
            new LoginForm().Show();
            this.Hide();
        }
    }
}