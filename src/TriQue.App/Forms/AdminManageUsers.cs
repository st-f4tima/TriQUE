using System.Linq.Expressions;
using TriQue;
using TriQue.Data.Repositories;
using TriQue.Helpers;
using TriQue.Helpers.Animation;
using TriQue.Services;

namespace TriQue.Forms
{
    public partial class AdminManageUsers : Form
    {
        private readonly UserService _userService = new();
        private readonly RotationService _rotationService = new();

        private int _userID;

        public AdminManageUsers(int userID)
        {
            InitializeComponent();
            ApplyFonts();

            _userID = userID;

            SetupGrid();
            LoadUsers();
        }

        #region Grid Setup
        private void SetupGrid()
        {
            UserListDataGrid.ReadOnly = true;
            UserListDataGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
            UserListDataGrid.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);

            AddTextColumn("UserID", visible: false);
            AddTextColumn("FullName", "Name", 180);
            AddTextColumn("PhoneNumber", "Phone #", 140);
            AddTextColumn("RoleName", "Role", 80);
            AddTextColumn("GroupName", "Group", 100);
            AddTextColumn("AssignedRoute", "Today's Route", 180, fill: true);
            AddTextColumn("Status", "Status", 80);

            UserListDataGrid.Columns.Add(MakeButtonColumn("EditCol", "✏ Edit", Color.FromArgb(220, 53, 69)));
            UserListDataGrid.Columns.Add(MakeButtonColumn("ViewCol", "👁 View", Color.FromArgb(13, 110, 253)));
        }

        private void AddTextColumn(string name, string header = "", int width = 0, bool fill = false, bool visible = true)
        {
            var col = new DataGridViewTextBoxColumn
            {
                Name = name,
                Visible = visible,
                AutoSizeMode = fill
                    ? DataGridViewAutoSizeColumnMode.Fill
                    : DataGridViewAutoSizeColumnMode.None
            };

            if (!string.IsNullOrEmpty(header)) col.HeaderText = header;
            if (width > 0) col.Width = width;
            if (width > 0) col.MinimumWidth = width;

            UserListDataGrid.Columns.Add(col);
        }

        private static DataGridViewButtonColumn MakeButtonColumn(string name, string text, Color backColor)
        {
            var col = new DataGridViewButtonColumn
            {
                Name = name,
                HeaderText = name == "EditCol" ? "Actions" : "",
                Text = text,
                UseColumnTextForButtonValue = true,
                Width = 100,
                MinimumWidth = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                FlatStyle = FlatStyle.Flat
            };

            col.DefaultCellStyle.BackColor = backColor;
            col.DefaultCellStyle.ForeColor = Color.White;
            col.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            col.DefaultCellStyle.SelectionBackColor = backColor;
            col.DefaultCellStyle.SelectionForeColor = Color.White;

            return col;
        }

        private void ApplyFonts()
        {
            this.Font = FontHelper.RobotoRegular;

            lblTitle.Font = FontHelper.GetRoboto(12f, FontStyle.Bold);
            SearchBar.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            AddUserBtn.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);

            UserListDataGrid.Font = FontHelper.RobotoRegular;
            UserListDataGrid.ColumnHeadersDefaultCellStyle.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            UserListDataGrid.DefaultCellStyle.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
        }

        #endregion

        private void LoadUsers(string search = "")
        {
            var users = _userService.GetAllUsers(search);
            UserListDataGrid.Rows.Clear();

            foreach (var u in users)
            {
                string assignedRoute = "—";

                if (u.RoleName == "Driver" && u.GroupID > 0)
                    assignedRoute = _rotationService.GetTodayRoute(u.GroupID)?.RouteName ?? "—";

                int rowIndex = UserListDataGrid.Rows.Add(
                    u.UserID,
                    u.FullName,
                    u.PhoneNumber,
                    u.RoleName,
                    u.GroupName,
                    assignedRoute,
                    u.Status
                );

                StyleRow(UserListDataGrid.Rows[rowIndex], u.Status);
            }
        }

        #region Row / Cell Styling

        private static void StyleRow(DataGridViewRow row, string status)
        {
            // Status cell
            var statusCell = row.Cells["Status"];
            statusCell.Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            statusCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            statusCell.Style.BackColor = Color.White;
            statusCell.Style.SelectionBackColor = Color.White;
            statusCell.Style.ForeColor = status switch
            {
                "Waiting" => Color.FromArgb(255, 193, 7),
                "OnTrip" => Color.FromArgb(40, 167, 69),
                "Finished" => Color.FromArgb(0, 123, 255),
                "Active" => Color.FromArgb(40, 167, 69),
                _ => Color.Gray
            };
            statusCell.Style.SelectionForeColor = statusCell.Style.ForeColor;

            StyleButtonCell(row.Cells["EditCol"], Color.FromArgb(220, 53, 69));
            StyleButtonCell(row.Cells["ViewCol"], Color.FromArgb(13, 110, 253));
        }

        private static void StyleButtonCell(DataGridViewCell cell, Color backColor)
        {
            cell.Style.BackColor = backColor;
            cell.Style.ForeColor = Color.White;
            cell.Style.SelectionBackColor = backColor;
            cell.Style.SelectionForeColor = Color.White;
            cell.Style.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        #endregion

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
                LoadUsers();
            }
            else if (colName == "ViewCol")
            {
                var modal = new UserDetailsModal(userID);

                await ModalAnimator.ShowModalAsync(this, modal);
            }
        }


        #region navigation
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
            if (MessageBox.Show("Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            new AuthenticationService().Logout(_userID);

            await FormAnimator.SwitchAsync(this, new LoginForm());
        }

        #endregion
    }
}
