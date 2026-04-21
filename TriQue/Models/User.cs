using System;
using TriQue.Enums;

namespace TriQue.Models
{
    public abstract class User
    {
        public required int UserID { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required UserRole Role { get; set; }
        public required int FailedAttempts { get; set; } = 0;
        public DateTime? LockoutUntil { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
