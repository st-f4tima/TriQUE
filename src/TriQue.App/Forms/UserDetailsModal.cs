using TriQue.Data.Repositories;
using TriQue.Services;

namespace TriQue
{
    public partial class UserDetailsModal : Form
    {
        private readonly UserRepository _repo = new();
        private readonly RotationService _rotationService = new();

        public UserDetailsModal(int userID)
        {
            InitializeComponent();
            LoadData(userID);
        }

        private void LoadData(int userID)
        {
            var d = _repo.GetUserDetail(userID);
            if (d == null) return;

            lblFullName.Text = d.FullName;
            lblRole.Text = d.RoleName;
            lblPhoneValue.Text = d.PhoneNumber;
            lblBodyValue.Text = string.IsNullOrEmpty(d.BodyNumber) ? "—" : d.BodyNumber;
            lblRoleValue.Text = d.RoleName;
            lblGroupNameValue.Text = string.IsNullOrEmpty(d.GroupName) ? "—" : d.GroupName;
            lblDriverStatus.Text = d.Status;

            if (d.RoleID == 1 && d.GroupID > 0)
            {
                var todayRoute = _rotationService.GetTodayRoute(d.GroupID);
                lblRouteValue.Text = todayRoute?.RouteName ?? "—";
            }
            else
            {
                lblRouteValue.Text = "—";
            }


            lblDriverStatus.ForeColor = d.Status switch
            {
                "OnTrip" => Color.FromArgb(40, 167, 69),
                "Waiting" => Color.FromArgb(255, 193, 7),
                "Finished" => Color.FromArgb(0, 123, 255),
                _ => Color.FromArgb(40, 167, 69),
            };

            lblDriverStatus.Text = d.Status switch
            {
                "OnTrip" => "On Trip",
                "Waiting" => "Waiting",
                "Finished" => "Finished",
                _ => "Active"
            };
        }
    }
}