using System;
using TriQue.Enums;

namespace TriQue.Models
{
    public abstract class User
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public int FailedAttempts { get; set; } = 0;
        public DateTime? LockoutUntil { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
