using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TriQue.Data.Repositories;
using TriQue.Enums;

namespace Trique.Forms
{
    public partial class QueueModal : Form
    {
        private readonly string _routeName;
        private readonly int _routeID;
        private readonly int _userID;
        private readonly AdminRepository _adminRepo = new();
        private DataTable _fullTable;
        private bool _isSuperAdmin;

        public QueueModal(string routeName, int routeID, int userID)
        {
            InitializeComponent();

            _routeName = routeName;
            _routeID = routeID;
            _userID = userID;
            this.Text = routeName;
            this.StartPosition = FormStartPosition.CenterScreen;

            CheckAdminLevel();
            SetupGrid();
            LoadQueue();
            SetupSearch();
        }

        private void CheckAdminLevel()
        {
            var level = _adminRepo.GetAdminLevel(_userID);
            _isSuperAdmin = level == AdminLevel.SuperAdmin;

            if (!_isSuperAdmin)
            {
                ResetQueueBtn.Enabled = false;
                ResetQueueBtn.FillColor = Color.Gray;
                ResetQueueBtn.ForeColor = Color.White;
            }
        }

        private void SetupGrid()
        {
            DriverListDataGrid.AutoGenerateColumns = false;
            DriverListDataGrid.AllowUserToAddRows = false;
            DriverListDataGrid.ReadOnly = false;

            var chk = new DataGridViewCheckBoxColumn
            {
                Name = "chkSelect",
                HeaderText = "",
                Width = 20,
                ReadOnly = false
            };

            var colRank = new DataGridViewTextBoxColumn
            {
                Name = "colRanking",
                HeaderText = "Ranking",
                DataPropertyName = "Ranking",
                Width = 30,
                ReadOnly = true
            };

            var colBody = new DataGridViewTextBoxColumn
            {
                Name = "colBody",
                HeaderText = "Body Number",
                DataPropertyName = "BodyNumber",
                Width = 110,
                ReadOnly = true
            };

            var colName = new DataGridViewTextBoxColumn
            {
                Name = "colName",
                HeaderText = "Driver Name",
                DataPropertyName = "DriverName",
                Width = 250,
                ReadOnly = true
            };

            var colStatus = new DataGridViewTextBoxColumn
            {
                Name = "colStatus",
                HeaderText = "Trip Status",
                DataPropertyName = "TripStatus",
                Width = 100,
                ReadOnly = true
            };

            var colAction = new DataGridViewComboBoxColumn
            {
                Name = "colAction",
                HeaderText = "Action",
                Width = 100,
                FlatStyle = FlatStyle.Flat,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
            };
            colAction.Items.AddRange("Waiting", "OnTrip", "Finished");

            DriverListDataGrid.Columns.AddRange(chk, colRank, colBody, colName, colStatus, colAction);
            DriverListDataGrid.CellFormatting += DgvQueue_CellFormatting;
        }

        private void LoadQueue()
        {
            _fullTable = _adminRepo.GetQueueByRouteID(_routeID);
            DriverListDataGrid.DataSource = _fullTable;

            foreach (DataGridViewRow row in DriverListDataGrid.Rows)
            {
                var status = row.Cells["colStatus"].Value?.ToString();

                switch (status)
                {
                    case "OnTrip":
                        row.Cells["colAction"].Value = "OnTrip";
                        break;

                    case "Finished":
                        row.Cells["colAction"].Value = "Finished";
                        break;

                    default:
                        row.Cells["colAction"].Value = "Waiting";
                        break;
                }
            }
        }

        private void SetupSearch()
        {
            SearchBar.TextChanged += (s, e) =>
            {
                string filter = SearchBar.Text.Trim().ToLower();

                if (string.IsNullOrEmpty(filter))
                {
                    DriverListDataGrid.DataSource = _fullTable;
                    return;
                }

                var rows = _fullTable.AsEnumerable()
                    .Where(r => r["DriverName"].ToString()
                        .ToLower().Contains(filter));

                DriverListDataGrid.DataSource = rows.Any()
                    ? rows.CopyToDataTable()
                    : _fullTable.Clone();
            };
        }

        private void DgvQueue_CellFormatting(object sender,
            DataGridViewCellFormattingEventArgs e)
        {
            if (DriverListDataGrid.Columns[e.ColumnIndex].Name != "colStatus") return;

            e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            var value = e.Value?.ToString();

            switch (value)
            {
                case "Waiting":
                    e.CellStyle.ForeColor = Color.Orange;
                    break;

                case "OnTrip":
                    e.CellStyle.ForeColor = Color.FromArgb(0, 150, 0);
                    break;

                case "Finished":
                    e.CellStyle.ForeColor = Color.FromArgb(0, 86, 179);
                    break;

                default:
                    e.CellStyle.ForeColor = Color.Black;
                    break;
            }
        }

        private void UpdateStatusBtn_Click_1(object sender, EventArgs e)
        {
            int updated = 0;

            foreach (DataGridViewRow row in DriverListDataGrid.Rows)
            {
                bool isChecked = Convert.ToBoolean(row.Cells["chkSelect"].Value);
                if (!isChecked) continue;

                var action = row.Cells["colAction"].Value?.ToString();
                int driverID = Convert.ToInt32(_fullTable.Rows[row.Index]["DriverID"]);

                int statusID;

                switch (action)
                {
                    case "OnTrip":
                        statusID = 2;
                        break;

                    case "Finished":
                        statusID = 3;
                        break;

                    default:
                        statusID = 1;
                        break;
                }

                _adminRepo.UpdateDriverStatus(driverID, statusID);
                updated++;
            }

            if (updated == 0)
            {
                MessageBox.Show("No drivers selected.",
                "Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show($"{updated} driver(s) updated.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            LoadQueue();
        }

        private void ResetQueueBtn_Click(object sender, EventArgs e)
        {
            if (!_isSuperAdmin)
            {
                MessageBox.Show("Only SuperAdmins can reset the queue.");
                return;
            }

            var confirm = MessageBox.Show(
                "Reset the queue for this route?",
                "Confirm Reset",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirm != DialogResult.Yes) return;

            _adminRepo.ResetQueue(_routeID);
            MessageBox.Show("Queue has been reset.!",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            LoadQueue();
        }

    }
}