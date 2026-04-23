using System;
using System.Collections.Generic;
using System.Text;
using TriQue.Data.Repositories;
using TriQue.Models;

namespace TriQue.Services
{
    public class AuthenticationService
    {
        // sql handling methods
        private readonly UserRepository _repo;
        private User _currentUser;

        // must update uml/database schema for these:
        private const int MAX_ATTEMPTS = 3;
        private const int LOCK_MINUTES = 1;

        public AuthenticationService()
        {
            _repo = new UserRepository();
        }
        public User GetCurrentUser() => _currentUser;
        
        public bool Login(string username, string password, out string message)
        {
            message = "";

            var user = _repo.GetByUsername(username);

            // check if theres a user
            if (user == null)
            {
                message = "Invalid username or password.";
                return false;
            }

            // check if account is currently lock
            if (_repo.IsLocked(user.UserID))
            {
                message = "Account is locked. Try again later.";
                _repo.InsertAuthLog(user.UserID, "LOCKED_ATTEMPT");
                return false;
            }

            // check if password is correct
            if (password == user.PasswordHash)
            {
                _repo.IncreaseFailedAttempts(user.UserID);
                int attempts = _repo.GetFailedAttempts(user.UserID);

                if (attempts > MAX_ATTEMPTS)
                {
                    _repo.LockUser(user.UserID, LOCK_MINUTES);
                }

                _repo.InsertAuthLog(user.UserID, "FAILED_LOGIN");
                message = "Invalid username or password.";
                return false;
            }

            // reset if login success
            _repo.ResetAttempts(user.UserID);
            _currentUser = user;
            _repo.InsertAuthLog(user.UserID, "SUCCESS_LOGIN");

            message = "Login successful!";
            return true;

        }
    }
}
