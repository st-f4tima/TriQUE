using TriQue.Data.Repositories;
using TriQue.Helpers;
using TriQue.Helpers.Animation;
using TriQue.Services;

namespace TriQue.Forms
{
    public partial class AdminSettings : Form
    {
        private AdminRepository _adminRepo = new();
        private int _userID;

        public AdminSettings(int userID)
        {
            InitializeComponent();
            ApplyFonts();
            
            _userID = userID;

            LoadSettings();
        }

        #region Load Methods

        private void LoadSettings()
        {
            LoadAdminProfile();
            LoadAdminList();
        }

        private void LoadAdminProfile()
        {
            var settings = _adminRepo.GetAdminSettings(_userID);

            if (settings != null)
            {
                lblFullName.Text = settings.Value.FullName;
                lblPhoneNumber.Text = settings.Value.PhoneNumber;
                lblAdminLevel.Text = settings.Value.LevelName.ToUpper();
            }
        }

        private void LoadAdminList()
        {
            SystemAdDataGrid.DataSource = _adminRepo.GetAllAdmins();

            SystemAdDataGrid.DataBindingComplete += SystemAdDataGrid_DataBindingComplete;
            SystemAdDataGrid.CellFormatting += SystemAdDataGrid_CellFormatting;
        }

        #endregion

        #region DataGrid Styling

        private void SystemAdDataGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (SystemAdDataGrid.Columns.Count < 3) return;

            SystemAdDataGrid.Columns[0].Width = 300;
            SystemAdDataGrid.Columns[1].Width = 250;
            SystemAdDataGrid.Columns[2].Width = 200;

            foreach (DataGridViewRow row in SystemAdDataGrid.Rows)
            {
                ApplyRoleStyle(row);
            }
        }

        private void SystemAdDataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex != 1 || e.RowIndex < 0) return;

            string level = SystemAdDataGrid.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";

            e.CellStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);

            switch (level)
            {
                case "SuperAdmin":
                    e.CellStyle.ForeColor = Color.FromArgb(34, 197, 94);
                    break;

                case "TodaOfficer":
                    e.CellStyle.ForeColor = Color.FromArgb(100, 88, 255);
                    break;

                default:
                    e.CellStyle.ForeColor = Color.FromArgb(156, 163, 175);
                    break;
            }

            e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
            e.FormattingApplied = true;
        }

        private static void ApplyRoleStyle(DataGridViewRow row)
        {
            string level = row.Cells[1].Value?.ToString() ?? "";

            var font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold);

            row.Cells[1].Style.Font = font;

            row.Cells[1].Style.ForeColor = level switch
            {
                "SuperAdmin" => Color.FromArgb(34, 197, 94),
                "TodaOfficer" => Color.FromArgb(100, 88, 255),
                _ => Color.FromArgb(156, 163, 175)
            };
        }

        private void ApplyFonts()
        {
            this.Font = FontHelper.RobotoRegular;

            lblTitle.Font = FontHelper.GetRoboto(12f, FontStyle.Bold);
            lblAdminInformationTItle.Font = FontHelper.GetRoboto(12f, FontStyle.Bold);
            lblFullName.Font = FontHelper.GetRoboto(20f, FontStyle.Bold);
            lblContactNumber.Font = FontHelper.GetRoboto(10f, FontStyle.Regular);
            lblUserRole.Font = FontHelper.GetRoboto(10f, FontStyle.Regular); 

            lblPhoneNumber.Font = FontHelper.GetRoboto(14f, FontStyle.Bold); 
            lblAdminLevel.Font = FontHelper.GetRoboto(14f, FontStyle.Bold);

            lblSystemAdministratorTitle.Font = FontHelper.GetRoboto(12f, FontStyle.Bold);

            SystemAdDataGrid.Font = FontHelper.RobotoRegular;
            SystemAdDataGrid.ColumnHeadersDefaultCellStyle.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            SystemAdDataGrid.DefaultCellStyle.Font = FontHelper.GetRoboto(9f, FontStyle.Bold);
        }

        #endregion

        #region Navigation

        //navbar
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

        private async void ViewQueueBtn_Click(object sender, EventArgs e)
        {
            await FormAnimator.SwitchAsync(this, new AdminViewQueue(_userID));
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

        private async void DashBtn_Click(object sender, EventArgs e)
        {
            await FormAnimator.SwitchAsync(this, new AdminForm(_userID));
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
    }
        #endregion
}