using System;
using TriQue.Data.Repositories;
using TriQue.Models;

namespace TriQue.Services
{
    public class AuthenticationService
    {
        private readonly UserRepository _repo;
        private User _currentUser;
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
            if (user == null)
            {
                message = "Invalid username or password.";
                return false;
            }

            // check lock before password
            if (_repo.IsLocked(user.UserID))
            {
                message = "Account is locked. Try again later.";
                _repo.InsertAuthLog(user.UserID, "LOCKED ATTEMPT");
                return false;
            }

            if (password != user.PasswordHash)
            {
                _repo.IncreaseFailedAttempts(user.UserID);

                int attempts = _repo.GetFailedAttempts(user.UserID);
                int remaining = MAX_ATTEMPTS - attempts;

                if (attempts >= MAX_ATTEMPTS)
                {
                    _repo.LockUser(user.UserID, LOCK_MINUTES);
                    _repo.InsertAuthLog(user.UserID, "LOCKED");
                    message = $"Account locked. Try again in {LOCK_MINUTES} minute(s).";
                }
                else
                {
                    _repo.InsertAuthLog(user.UserID, "FAILED");
                    message = $"Invalid username or password. {remaining} attempt(s) remaining before lockout.";
                }

                return false;
            }

            // success
            _repo.ResetAttempts(user.UserID);
            _currentUser = user;
            _repo.InsertAuthLog(user.UserID, "SUCCESS");

            message = "Login successful!";
            return true;
        }

        public int GetLockSecondsRemaining(string username)
        {
            var user = _repo.GetByUsername(username);
            if (user == null) return 0;

            var lockoutUntil = _repo.GetLockoutUntil(user.UserID);
            if (lockoutUntil == null) return 0;

            var remaining = (lockoutUntil.Value - DateTime.Now).TotalSeconds;
            return remaining > 0 ? (int)remaining : 0;
        }
    }
}