using TriQue.Data.Repositories;
using TriQue.DTOs;

namespace TriQue.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepo;

        public UserService()
        {
            _userRepo = new UserRepository();
        }

        public List<UserListDto> GetAllUsers(string search = "")
        {
             return _userRepo.GetAllUsers(search);
        }
           
    }
}
