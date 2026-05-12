using TriQue.Data.Repositories;
using TriQue.Forms;
using TriQue.Helpers.Animation;
using TriQue.Services;

namespace TriQue.Forms
{
    public partial class AdminViewQueue : Form
    {
        private readonly RotationService _rotationService;
        private readonly DriverRepository _driverRepo;
        private readonly int _userID;

        public AdminViewQueue(int userID)
        {
            InitializeComponent();

            _rotationService = new RotationService();
            _driverRepo = new DriverRepository();
            _userID = userID;
        }

        private async void btnRouteA_Click(object sender, EventArgs e)
        {
            int? groupID = _rotationService.GetGroupIDForRouteToday(101);
            if (groupID == null) { 
                MessageBox.Show("No group assigned to this route today."); 
                return; 
            }
            await ModalAnimator.ShowModalAsync(this, new QueueModal("Provincial Capitol", 101, _userID, groupID.Value));
        }

        private async void btnRouteB_Click(object sender, EventArgs e)
        {
            int? groupID = _rotationService.GetGroupIDForRouteToday(102);
            if (groupID == null)
            {
                MessageBox.Show("No group assigned to this route today.");
                return;
            }
            await ModalAnimator.ShowModalAsync(this, new QueueModal("Grand Terminal", 102, _userID, groupID.Value));
        }

        private async void btnRouteC_Click(object sender, EventArgs e)
        {
            int? groupID = _rotationService.GetGroupIDForRouteToday(103);
            if (groupID == null)
            {
                MessageBox.Show("No group assigned to this route today.");
                return;
            }
            await ModalAnimator.ShowModalAsync(this, new QueueModal("SM Batangas", 103, _userID, groupID.Value));
        }

        private async void btnRouteD_Click(object sender, EventArgs e)
        {
            int? groupID = _rotationService.GetGroupIDForRouteToday(104);
            if(groupID == null) {
                MessageBox.Show("No group assigned to this route today.");
                return;
            }
            await ModalAnimator.ShowModalAsync(this, new QueueModal("WalterMart", 104, _userID, groupID.Value));
        }

        private async void btnRouteE_Click(object sender, EventArgs e)
        {
            int? groupID = _rotationService.GetGroupIDForRouteToday(105);
            if (groupID == null)
            {
                MessageBox.Show("No group assigned to this route today.");
                return;
            }
            await ModalAnimator.ShowModalAsync(this, new QueueModal("Brgy. Tulo", 105, _userID, groupID.Value));
        }

        private async void btnRouteF_Click(object sender, EventArgs e)
        {
            int? groupID = _rotationService.GetGroupIDForRouteToday(106);
            if (groupID == null)
            {
                MessageBox.Show("No group assigned to this route today.");
                return;
            }
            await ModalAnimator.ShowModalAsync(this, new QueueModal("BSU Alangilan", 106, _userID, groupID.Value));
        }

        // navbar
        private async void DashBtn_Click(object sender, EventArgs e)
        {
            await FormAnimator.SwitchAsync(this, new AdminForm(_userID));
        }

        private async void ManageUserBtn_Click(object sender, EventArgs e)
        {
            var repo = new UserRepository();
            int level = repo.GetAdminLevel(_userID);

            // 1 = SuperAdmin only
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

        private async void GenerateReportBtn_Click(object sender, EventArgs e)
        {
            var repo = new UserRepository();
            int level = repo.GetAdminLevel(_userID);

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

        private async void LogoutBtn_Click(object sender, EventArgs e)
        {
            var authService = new AuthenticationService();
            authService.Logout(_userID);

            await FormAnimator.SwitchAsync(this, new LoginForm());
        }
    }
}
