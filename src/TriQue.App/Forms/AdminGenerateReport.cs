using TriQue.Data.Repositories;
using TriQue.DTOs;
using TriQue.Helpers;
using TriQue.Helpers.Animation;
using TriQue.Models;
using TriQue.Services;

namespace TriQue.Forms
{
    public partial class AdminGenerateReport : Form
    {
        private readonly ReportService _reportService = new();
        private readonly TripRepository _tripRepo = new();
        private readonly RouteRepository _routeRepo = new();
        private readonly DriverRepository _driverRepo = new();

        private readonly int _userID;

        public AdminGenerateReport(int userID)
        {
            InitializeComponent();
            ApplyFonts();

            _userID = userID;

            InitializeFilters();
            LoadDropdowns();
            LoadDefaultStats();
        }

        private void InitializeFilters()
        {
            dtpFrom.Value = DateTime.Today.AddMonths(-1);
            dtpTo.Value = DateTime.Today;

            LoadReportTypeDropdown();
        }

        #region Dropdowns
        private void LoadDropdowns()
        {
            LoadRouteDropdown();
            LoadDriverDropdown();
        }

        private void LoadReportTypeDropdown()
        {
            cmbReportType.Items.Clear();

            cmbReportType.Items.Add("Trip Summary");
            cmbReportType.Items.Add("Driver Performance");

            cmbReportType.SelectedIndex = 0;
        }

        private void LoadRouteDropdown()
        {
            cmbRoute.Items.Clear();

            cmbRoute.Items.Add(new Route
            {
                RouteID = 0,
                RouteName = "All Routes"
            });

            foreach (var route in _routeRepo.GetAllRoutes())
                cmbRoute.Items.Add(route);

            cmbRoute.DisplayMember = "RouteName";
            cmbRoute.SelectedIndex = 0;
        }

        private void LoadDriverDropdown()
        {
            cmbDriver.Items.Clear();
            cmbDriver.Items.Add(new { DriverID = 0, FullName = "All Drivers" });

            var drivers = _driverRepo.GetAllDrivers();
            foreach (var d in drivers)
            {
                cmbDriver.Items.Add(d);
            }

            cmbDriver.DisplayMember = "FullName";
            cmbDriver.SelectedIndex = 0;
            cmbDriver.DropDownHeight = 150;
            cmbDriver.MaxDropDownItems = 5;
        }

        #endregion

        private void LoadDefaultStats()
        {
            LoadStats(null, null, null, null);
        }

        private void LoadStats(DateTime? from, DateTime? to, int? routeID, int? driverID)
        {
            var stats = _tripRepo.GetReportStats(from, to, routeID, driverID);

            lblTotalTrips.Text = stats.totalTrips.ToString();
            lblMostActive.Text = stats.mostActive;
            lblLeastActive.Text = stats.leastActive;
            lblFastestTrip.Text = $"{stats.fastest:0} min";
            lblSlowestTrip.Text = $"{stats.slowest:0} min";
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            DateTime from = dtpFrom.Value.Date;
            DateTime to = dtpTo.Value.Date;

            int? routeID = null;
            int? driverID = null;

            // Get selected route
            if (cmbRoute.SelectedItem is Route route && route.RouteID != 0)
            {
                routeID = route.RouteID;
            }

            // Get selected driver
            if (cmbDriver.SelectedItem is DriverDto driver && driver.DriverID != 0)
            {
                driverID = driver.DriverID;
            }

            // Get report type
            string reportType = cmbReportType.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(reportType))
            {
                MessageBox.Show("Please select a report type.");
                return;
            }
            // Update dashboard stats
            LoadStats(from, to, routeID, driverID);

            try
            {
                string path = "";

                // Choose which report to generate
                if (reportType == "Driver Performance")
                {
                    path = _reportService.GenerateDriverPerformancePdf(
                        from, to, routeID, driverID, "Admin"
                    );
                }
                else
                {
                    path = _reportService.GenerateTripSummaryPdf(
                        from, to, routeID, driverID, "Admin"
                    );
                }

                // Open the file
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true
                });

                MessageBox.Show(
                    "Report saved to:\n" + path,
                    "Report Generated",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to generate report:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        #region additional style

        private void ApplyFonts()
        {
  
            this.Font = FontHelper.RobotoRegular;

            lblReportTitle.Font = FontHelper.GetRoboto(12f, FontStyle.Bold);
            lblFilterHeading.Font = FontHelper.GetRoboto(12f, FontStyle.Bold);

            lblFromDate.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblToDate.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            dtpFrom.Font = FontHelper.GetRoboto(9f, FontStyle.Bold);
            dtpTo.Font = FontHelper.GetRoboto(9f, FontStyle.Bold);

            cmbReportType.Font = FontHelper.GetRoboto(9f, FontStyle.Bold);

            lblRoute.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            cmbRoute.Font = FontHelper.GetRoboto(9f, FontStyle.Bold);

            lblDriver.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            cmbDriver.Font = FontHelper.GetRoboto(9f, FontStyle.Bold);

            lblTotalTrips.Font = FontHelper.GetRoboto(18f, FontStyle.Bold);
            lblMostActive.Font = FontHelper.GetRoboto(11f, FontStyle.Bold);
            lblLeastActive.Font = FontHelper.GetRoboto(11f, FontStyle.Bold);
            lblFastestTrip.Font = FontHelper.GetRoboto(18f, FontStyle.Bold);
            lblSlowestTrip.Font = FontHelper.GetRoboto(18f, FontStyle.Bold);

            lblTotalTripsLabel.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblMostActiveLabel.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblLeastActiveLabel.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblFastestTripLabel.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblSlowestTripLabel.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);

            btnGenerateReport.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
        }

        #endregion


        #region navigation
        private async void ViewQueueBtn_Click(object sender, EventArgs e)
        {
            await FormAnimator.SwitchAsync(this, new AdminViewQueue(_userID));
        }

        private async void ManageUserBtn_Click(object sender, EventArgs e)
        {
            var repo = new UserRepository();
            int level = repo.GetAdminLevel(_userID);

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

        private async void SettingsBtn_Click(object sender, EventArgs e)
        {
            await FormAnimator.SwitchAsync(this, new AdminSettings(_userID));
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

        #endregion
    }
}