using TriQue.Data.Repositories;
using TriQue.DTOs;
using TriQue.Forms;
using TriQue.Helpers.Animation;
using TriQue.Models;
using TriQue.Services;

namespace Trique.Forms
{
    public partial class AdminGenerateReport : Form
    {
        private ReportService _reportService;
        private TripRepository _tripRepo;
        private RouteRepository _routeRepo;
        private DriverRepository _driverRepo;
        private int _userID;

        public AdminGenerateReport(int userID)
        {
            InitializeComponent();
            _userID = userID;
            _reportService = new ReportService();
            _tripRepo = new TripRepository();
            _routeRepo = new RouteRepository();
            _driverRepo = new DriverRepository();

            LoadRouteDropdown();
            LoadDriverDropdown();

            dtpFrom.Value = DateTime.Today.AddMonths(-1);
            dtpTo.Value = DateTime.Today;

            LoadStats(null, null, null, null);
        }

        private void LoadRouteDropdown()
        {
            cmbRoute.Items.Clear();
            cmbRoute.Items.Add(new Route { RouteID = 0, RouteName = "All Routes" });

            var routes = _routeRepo.GetAllRoutes();
            foreach (var r in routes)
                cmbRoute.Items.Add(r);

            cmbRoute.DisplayMember = "RouteName";
            cmbRoute.SelectedIndex = 0;
        }

        private void LoadDriverDropdown()
        {
            cmbDriver.Items.Clear();
            cmbDriver.Items.Add(new { DriverID = 0, FullName = "All Drivers" });

            var drivers = _driverRepo.GetAllDrivers(); 
            foreach (var d in drivers)
                cmbDriver.Items.Add(d);

            cmbDriver.DisplayMember = "FullName";
            cmbDriver.SelectedIndex = 0;
            cmbDriver.DropDownHeight = 150; 
            cmbDriver.MaxDropDownItems = 5;
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
            DateTime? from = dtpFrom.Value.Date;
            DateTime? to = dtpTo.Value.Date;
            int? routeID = cmbRoute.SelectedItem is Route r && r.RouteID != 0 ? r.RouteID : null;
            int? driverID = cmbDriver.SelectedItem is DriverDto d && d.DriverID != 0 ? d.DriverID : null;


            LoadStats(from, to, routeID, driverID);
            
            // generate report
            try
            {
                string path = _reportService.GenerateTripSummaryPdf(
                    from, to, routeID, driverID,
                    generatedBy: "Admin"
                );

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true
                });

                MessageBox.Show(
                    $"Report saved to:\n{path}",
                    "Report Generated",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to generate report:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // navbar
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
            await FormAnimator.SwitchAsync(this, new AdminViewQueue(_userID));
        }

        private async void LogoutBtn_Click(object sender, EventArgs e)
        {
            var authService = new AuthenticationService();
            authService.Logout(_userID);

            await FormAnimator.SwitchAsync(this, new LoginForm());
        }
    }
}