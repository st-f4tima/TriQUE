using TriQue.Helpers;
using TriQue.Services;

namespace TriQue
{
    public partial class UserDetailsModal : Form
    {
        private readonly UserService _userService = new();
        private readonly RotationService _rotationService = new();

        public UserDetailsModal(int userID)
        {
            InitializeComponent();
            ApplyFonts();

            LoadUserDetails(userID);
        }

        #region Load Methods

        private void LoadUserDetails(int userID)
        {
            var user = _userService.GetUserDetail(userID);

            if (user == null)
                return;

            DisplayBasicInformation(user);
            DisplayRouteInformation(user);
            DisplayDriverStatus(user.Status);
        }
        private void DisplayBasicInformation(dynamic user)
        {
            lblFullName.Text = user.FullName;

            lblRole.Text = user.RoleName;
            lblRoleValue.Text = user.RoleName;

            lblPhoneValue.Text = user.PhoneNumber;

            lblBodyValue.Text = string.IsNullOrWhiteSpace(user.BodyNumber)
                ? "—"
                : user.BodyNumber;

            lblGroupNameValue.Text = string.IsNullOrWhiteSpace(user.GroupName)
                ? "—"
                : user.GroupName;
        }

        private void DisplayRouteInformation(dynamic user)
        {
            bool isDriverWithGroup =
                user.RoleID == 1 &&
                user.GroupID > 0;

            if (!isDriverWithGroup)
            {
                lblRouteValue.Text = "—";
                return;
            }

            var route = _rotationService.GetTodayRoute(user.GroupID);

            lblRouteValue.Text = route?.RouteName ?? "—";
        }

        private void DisplayDriverStatus(string status)
        {
            lblDriverStatus.ForeColor = status switch
            {
                "OnTrip" => Color.FromArgb(40, 167, 69),
                "Waiting" => Color.FromArgb(255, 193, 7),
                "Finished" => Color.FromArgb(0, 123, 255),
                _ => Color.FromArgb(40, 167, 69),
            };

            lblDriverStatus.Text = status switch
            {
                "OnTrip" => "On Trip",
                "Waiting" => "Waiting",
                "Finished" => "Finished",
                _ => "Active"
            };
        }

        #endregion

        private void ApplyFonts()
        {
            this.Font = FontHelper.RobotoRegular;

            lblFullName.Font = FontHelper.GetRoboto(20f, FontStyle.Bold);
            lblRole.Font = FontHelper.GetRoboto(10f, FontStyle.Regular);
            lblRole.ForeColor = Color.DimGray;

            lblPhoneNumber.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblBodyNumber.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblRole.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblGroup.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblAssignedRoute.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblStatus.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);

            lblPhoneValue.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblBodyValue.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblGroupNameValue.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblRouteValue.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
            lblDriverStatus.Font = FontHelper.GetRoboto(10f, FontStyle.Bold);
        }
    }
}