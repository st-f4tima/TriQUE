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
           
            // Todo: Put it inside here
        }

    }
}
