using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using TriQue.Data.Repositories;
using TriQue.Forms;

namespace Trique.Forms
{
    public partial class AdminSettings : Form
    {
        private int _userID;
        private AdminRepository _adminRepo;

        public AdminSettings(int userID)
        {
            InitializeComponent();
            _userID = userID;
            _adminRepo = new AdminRepository();
            LoadSettings();
        }

        private void LoadSettings()
        {
            var settings = _adminRepo.GetAdminSettings(_userID);
            if (settings != null)
            {
                lblFullName.Text = settings.Value.FullName;
                lblPhoneNumber.Text = settings.Value.PhoneNumber;
                lblAdminLevel.Text = settings.Value.LevelName.ToUpper();
            }

            SystemAdDataGrid.DataSource = _adminRepo.GetAllAdmins();
            SystemAdDataGrid.DataBindingComplete += (s, e) =>
            {
                if (SystemAdDataGrid.Columns.Count < 3) return;

                SystemAdDataGrid.Columns[0].Width = 300;
                SystemAdDataGrid.Columns[1].Width = 250;
                SystemAdDataGrid.Columns[2].Width = 200;

                // color Authorization Level column text
                foreach (DataGridViewRow row in SystemAdDataGrid.Rows)
                {
                    string level = row.Cells[1].Value?.ToString() ?? "";

                    if (level == "SuperAdmin")
                    {
                        row.Cells[1].Style.ForeColor = Color.FromArgb(34, 197, 94); // green
                        row.Cells[1].Style.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold);
                    }
                    else if (level == "TodaOfficer")
                    {
                        row.Cells[1].Style.ForeColor = Color.FromArgb(100, 88, 255); // purple
                        row.Cells[1].Style.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold);
                    }
                    else
                    {
                        row.Cells[1].Style.ForeColor = Color.FromArgb(156, 163, 175); // gray
                        row.Cells[1].Style.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold);
                    }
                }

                // selected row text color keeps changing...
                SystemAdDataGrid.CellFormatting += (s, e) =>
                {
                    if (e.ColumnIndex != 1 || e.RowIndex < 0) return;

                    string level = SystemAdDataGrid.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";

                    if (level == "SuperAdmin")
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(34, 197, 94);
                        e.CellStyle.SelectionForeColor = Color.FromArgb(34, 197, 94);
                        e.CellStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
                    }
                    else if (level == "TodaOfficer")
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(100, 88, 255);
                        e.CellStyle.SelectionForeColor = Color.FromArgb(100, 88, 255);
                        e.CellStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
                    }
                    else
                    {
                        e.CellStyle.ForeColor = Color.FromArgb(156, 163, 175);
                        e.CellStyle.SelectionForeColor = Color.FromArgb(156, 163, 175);
                        e.CellStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
                    }

                    e.FormattingApplied = true;
                };
            };
        }

        private void GenerateReportBtn_Click(object sender, EventArgs e)
        {
            AdminGenerateReport reportForm = new AdminGenerateReport(_userID);
            reportForm.Show();
            this.Hide();
        }

        private void ViewQueueBtn_Click(object sender, EventArgs e)
        {
            AdminViewQueue adminForm = new AdminViewQueue(_userID);
            adminForm.Show();
            this.Hide();
        }

        private void ManageUsersBtn_Click(object sender, EventArgs e)
        {
            AdminManageUsers adminForm = new AdminManageUsers(_userID);
            adminForm.Show();
            this.Hide();
        }

        private void DashBtn_Click(object sender, EventArgs e)
        {
            AdminViewQueue adminForm = new AdminViewQueue(_userID);
            adminForm.Show();
            this.Hide();
        }

        private void LogoutBtn_Click(object sender, EventArgs e)
        {
            LoginForm adminForm = new LoginForm();
            adminForm.Show();
            this.Hide();
        }
    }
}