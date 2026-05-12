using System;
using TriQue.Data.Repositories;
using TriQue.Enums;
using TriQue.Helpers;
using TriQue.Models;

namespace TriQue.Services
{
    public class AuthenticationService
    {
        private readonly UserRepository _userRepo;
        private const int MAX_ATTEMPTS = 3;
        private const int LOCK_MINUTES = 1;

        private User _currentUser;

        public AuthenticationService()
        {
            _userRepo = new UserRepository();
        }

        public User GetCurrentUser() => _currentUser;

        public bool CurrentUserNeedsPasswordReset()
        {
            if (_currentUser == null) return false;
            return _userRepo.IsTemporaryPassword(_currentUser.UserID);
        }

        public bool Login(string username, string password, out string message)
        {
            message = "";

            var user = _userRepo.GetByUsername(username);
            if (user == null)
            {
                message = "Invalid username or password.";
                return false;
            }

            // check lock before password
            if (_userRepo.IsLocked(user.UserID))
            {
                message = "Account is locked. Try again later.";
                Log(user.UserID, AuthOutcome.LockedAttempt);
                return false;
            }

            if (!PasswordHelper.Verify(password, user.PasswordHash))
            {
                _userRepo.IncreaseFailedAttempts(user.UserID);

                int attempts = _userRepo.GetFailedAttempts(user.UserID);
                int remaining = MAX_ATTEMPTS - attempts;

                if (attempts >= MAX_ATTEMPTS)
                {
                    _userRepo.LockUser(user.UserID, LOCK_MINUTES);
                    Log(user.UserID, AuthOutcome.Locked);
                    message = $"Account locked. Try again in {LOCK_MINUTES} minute(s).";
                }
                else
                {
                    Log(user.UserID, AuthOutcome.Failed);
                    message = $"Invalid username or password. {remaining} attempt(s) remaining before lockout.";
                }

                return false;
            }

            // success
            _userRepo.ResetAttempts(user.UserID);
            _currentUser = user;
            Log(user.UserID, AuthOutcome.Success);

            message = "Login successful!";
            return true;
        }


        public int GetLockSecondsRemaining(string username)
        {
            var user = _userRepo.GetByUsername(username);
            if (user == null) return 0;

            var lockoutUntil = _userRepo.GetLockoutUntil(user.UserID);
            if (lockoutUntil == null) return 0;

            var remaining = (lockoutUntil.Value - DateTime.Now).TotalSeconds;
            return remaining > 0 ? (int)remaining : 0;
        }

        public void Log(int userId, AuthOutcome outcome)
        {
            _userRepo.InsertAuthLog(new AuthenticationLog
            {
                UserID = userId,
                LoginTime = DateTime.Now,
                AuthOutcome = outcome.ToString()
            });
        }

        public void Logout(int userID)
        {
            var log = new AuthenticationLog
            {
                UserID = userID,
                LogoutTime = DateTime.Now
            };
            _userRepo.InsertLogoutLog(log);
            _currentUser = null;
        }
    }
}