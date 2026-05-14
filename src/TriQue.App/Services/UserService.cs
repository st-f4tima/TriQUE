using TriQue.Data.Repositories;
using TriQue.DTOs;
using TriQue.Helpers;

namespace TriQue.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepo;

        public UserService()
        {
            _userRepo = new UserRepository();
        }

        public UserDetailDto? GetUserDetail(int userID)
        {
            return _userRepo.GetUserDetail(userID);
        }

        public int GetAdminLevel(int userID)
        {
            return _userRepo.GetAdminLevel(userID);
        }

        public List<UserListDto> GetAllUsers(string search = "")
        {
             return _userRepo.GetAllUsers(search);
        }

        public CreatedUserDto AddUser(string firstName, string lastName, string phone, int roleID, int groupID, int levelID = 3)
        {
            return _userRepo.AddUser(firstName, lastName, phone, roleID, groupID, levelID);
        }
        public void UpdateUser(int userID, string fullName, string phoneNumber, int roleID, int groupID, int adminLevelID)
        {
            _userRepo.UpdateUser(userID, fullName, phoneNumber, roleID, groupID, adminLevelID);
        }

        public void DeleteUser(int userID)
        {
            _userRepo.DeleteUser(userID);
        }
    }
}
