using TriQue.Data.Repositories;
using TriQue.Forms;
using TriQue.Models;
using TriQue.Services;
using TriQue.DTOs;

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

        // navigation
        private void ViewQueueBtn_Click(object sender, EventArgs e)
        {
            AdminViewQueue adminForm = new AdminViewQueue(_userID);
            adminForm.Show();
            this.Hide();
        }

        private void ManageUserBtn_Click(object sender, EventArgs e)
        {
            AdminManageUsers adminForm = new AdminManageUsers(_userID);
            adminForm.Show();
            this.Hide();
        }

        private void SettingsBtn_Click(object sender, EventArgs e)
        {
            AdminSettings settingsForm = new AdminSettings(_userID);
            settingsForm.Show();
            this.Hide();
        }

        private void GenerateReportBtn_Click(object sender, EventArgs e)
        {
            AdminGenerateReport reportForm = new AdminGenerateReport(_userID);
            reportForm.Show();
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