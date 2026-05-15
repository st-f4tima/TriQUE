using System.Data;
using TriQue.Data.Repositories;
using TriQue.Enums;
using TriQue.Helpers;
using TriQue.Models;
using TriQue.Services;

namespace TriQue.Forms
{
    public partial class QueueModal : Form
    {
        private readonly TripService _tripService = new();
        private readonly AdminService _adminService = new();
        private readonly QueueService _queueService = new();
        private readonly DriverService _driverService = new();

        private readonly int _routeID;
        private readonly int _userID;
        private readonly int _groupID;

        private DataTable _fullTable;
        private bool _isSuperAdmin;

        public QueueModal(string routeName, int routeID, int userID, int groupID)
        {
            InitializeComponent();
            ApplyFonts();

            _routeID = routeID;
            _userID = userID;
            _groupID = groupID;

            this.Text = routeName;
            this.StartPosition = FormStartPosition.CenterScreen;

            LoadQueueModal();
        }

        private void LoadQueueModal()
        {
            CheckAdminLevel();
            SetupGrid();
            LoadQueue();
            SetupSearch();
        }

        private void CheckAdminLevel()
        {
            var level = _adminService.GetAdminLevel(_userID);
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
        }

        private void LoadQueue()
        {
            _fullTable = _queueService.GetQueueByGroupID(_groupID, _routeID);
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

        private void DgvQueue_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var colName = DriverListDataGrid.Columns[e.ColumnIndex].Name;

            // hide ranking for Finished drivers
            if (colName == "colRanking")
            {
                var row = DriverListDataGrid.Rows[e.RowIndex];
                var status = row.Cells["colStatus"].Value?.ToString();
                if (status == "Finished")
                    e.Value = "-";
                return;
            }

            if (colName != "colStatus") return;

            e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            var value = e.Value?.ToString();

            Color statusColor = value switch
            {
                "Waiting" => Color.Orange,
                "OnTrip" => Color.FromArgb(0, 150, 0),
                "Finished" => Color.FromArgb(0, 86, 179),
                _ => Color.FromArgb(91, 91, 91)
            };

            e.CellStyle.ForeColor = statusColor;
            e.CellStyle.SelectionForeColor = statusColor;
            e.CellStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
        }

        private void ApplyFonts()
        {
            this.Font = FontHelper.RobotoRegular;

            SearchBar.Font = FontHelper.GetRoboto(9f, FontStyle.Bold);
            UpdateStatusBtn.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            ResetQueueBtn.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);

            DriverListDataGrid.Font = FontHelper.RobotoRegular;
            DriverListDataGrid.ColumnHeadersDefaultCellStyle.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            DriverListDataGrid.DefaultCellStyle.Font = FontHelper.GetRoboto(8f, FontStyle.Bold);

            if (DriverListDataGrid.Columns.Contains("colAction"))
            {
                DriverListDataGrid.Columns["colAction"].DefaultCellStyle.Font = FontHelper.GetRoboto(8f, FontStyle.Bold);
            }
        }

        #region Queue actions

        private void UpdateStatusBtn_Click_1(object sender, EventArgs e)
        {
            int updated = 0;

            foreach (DataGridViewRow row in DriverListDataGrid.Rows)
            {
                bool isChecked = Convert.ToBoolean(row.Cells["chkSelect"].Value);
                if (!isChecked) continue;

                var action = row.Cells["colAction"].Value?.ToString();
                int driverID = Convert.ToInt32(_fullTable.Rows[row.Index]["DriverID"]);

                switch (action)
                {
                    case "OnTrip":
                        _tripService.StartTrip(driverID, _routeID);
                        _driverService.UpdateStatus(driverID, DriverStatus.OnTrip);
                        break;

                    case "Finished":
                        _tripService.EndTrip(driverID, _routeID);
                        _driverService.UpdateStatus(driverID, DriverStatus.Waiting); // ← Waiting
                        int queueID1 = _queueService.GetQueueIdByRouteId(_routeID) ?? 0;
                        _queueService.RemoveDriverFromQueue(driverID, queueID1);
                        _queueService.ReorderQueuePositions(queueID1);
                        break;

                    default: 
                        _tripService.EndTrip(driverID, _routeID);
                        _driverService.UpdateStatus(driverID, DriverStatus.Waiting);
                        int queueID2 = _queueService.GetQueueIdByRouteId(_routeID) ?? 0;
                        _queueService.RemoveDriverFromQueue(driverID, queueID2);
                        _queueService.ReorderQueuePositions(queueID2);
                        break;
                }

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

            _queueService.ResetQueue(_routeID, _groupID);
            MessageBox.Show("Queue has been reset",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            LoadQueue();
        }

        #endregion

    }
}