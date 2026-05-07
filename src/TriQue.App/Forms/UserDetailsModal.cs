using TriQue.Data.Repositories;

namespace TriQue
{
    public partial class UserDetailsModal : Form
    {
        private readonly UserRepository _repo = new();

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
            lblRouteValue.Text = d.AssignedRoute;
            lblStatusValue.Text = d.Status;

            lblStatusValue.FillColor = d.Status switch
            {
                "OnTrip" => Color.FromArgb(25, 135, 84),
                "Waiting" => Color.FromArgb(255, 193, 7),
                "Finished" => Color.FromArgb(13, 110, 253),
                _ => Color.FromArgb(108, 117, 125)
            };

            lblDriverStatus.Text = d.Status switch
            {
                "OnTrip" => "On Trip",
                "Waiting" => "Waiting",
                "Finished" => "Finished",
                _ => "Active"
            };

            lblStatusValue.ForeColor = Color.White;
        }
    }
}