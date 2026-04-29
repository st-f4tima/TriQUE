using Microsoft.Data.Sqlite;
using System;
using System.Data;
using TriQue.Enums;
using TriQue.Helpers;
using TriQue.Models;

namespace TriQue.Data.Repositories
{
    public class UserRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public UserRepository()
        {
            _dbHelper = new DatabaseHelper();
        }

        public User? GetById(int userID)
        {
            string query = @"
                SELECT UserID, Username, PasswordHash, FirstName, LastName, PhoneNumber, RoleID
                FROM User
                WHERE UserID = @id
                LIMIT 1";

            var param = new[] { new SqliteParameter("@id", userID) };

            using var reader = _dbHelper.ExecuteReader(query, param);

            if (!reader.Read()) return null;

            int roleId = Convert.ToInt32(reader["RoleID"]);

            User user = roleId == 2 ? new Admin() : new Driver();

            user.UserID = Convert.ToInt32(reader["UserID"]);
            user.Username = reader["Username"].ToString() ?? "";
            user.PasswordHash = reader["PasswordHash"].ToString() ?? "";
            user.FirstName = reader["FirstName"].ToString() ?? "";
            user.LastName = reader["LastName"].ToString() ?? "";
            user.PhoneNumber = reader["PhoneNumber"].ToString() ?? "";
            user.Role = (UserRole)roleId;

            return user;
        }
        public User? GetByUsername(string username)
        {
            string query = @"
                SELECT UserID, Username, PasswordHash, RoleID
                FROM User
                WHERE Username = @username
                LIMIT 1";

            var param = new[] { new SqliteParameter("@username", username) };

            using var reader = _dbHelper.ExecuteReader(query, param);

            if (!reader.Read()) return null;

            int roleId = Convert.ToInt32(reader["RoleID"]);
            User user = roleId == 2 ? new Admin() : new Driver();

            user.UserID = Convert.ToInt32(reader["UserID"]);
            user.Username = reader["Username"].ToString() ?? "";
            user.PasswordHash = reader["PasswordHash"].ToString() ?? "";

            return user;
        }

        public int GetFailedAttempts(int userID)
        {
            string query = "SELECT FailedAttempts FROM User WHERE UserID = @id";
            var result = _dbHelper.ExecuteScalar(query, new SqliteParameter("@id", userID));
            return Convert.ToInt32(result);
        }

        public void IncreaseFailedAttempts(int userID)
        {
            string query = @"
                UPDATE User
                SET FailedAttempts = FailedAttempts + 1
                WHERE UserID = @id";

            _dbHelper.ExecuteNonQuery(query, new SqliteParameter("@id", userID));
        }

        public void ResetAttempts(int userID)
        {
            string query = @"
                UPDATE User
                SET FailedAttempts = 0,
                    LockoutUntil = NULL
                WHERE UserID = @id";

            _dbHelper.ExecuteNonQuery(query, new SqliteParameter("@id", userID));
        }

        public void LockUser(int userID, int minutes)
        {
            string query = @"
                UPDATE User
                SET FailedAttempts = 0,
                    LockoutUntil = @lock
                WHERE UserID = @id";

            _dbHelper.ExecuteNonQuery(query,
                new SqliteParameter("@lock", DateTime.Now.AddMinutes(minutes).ToString("yyyy-MM-dd HH:mm:ss")),
                new SqliteParameter("@id", userID));
        }

        public bool IsLocked(int userID)
        {
            var lockoutUntil = GetLockoutUntil(userID);
            return lockoutUntil.HasValue && lockoutUntil.Value > DateTime.Now;
        }

        public DateTime? GetLockoutUntil(int userID)
        {
            string query = "SELECT LockoutUntil FROM User WHERE UserID = @id";
            var result = _dbHelper.ExecuteScalar(query, new SqliteParameter("@id", userID));

            if (result == null || result == DBNull.Value)
                return null;

            if (DateTime.TryParse(result.ToString(), out DateTime dt))
                return dt;

            return null;
        }

        public void InsertAuthLog(int userID, string outcome)
        {
            string query = @"
                INSERT INTO AuthenticationLog (UserID, LoginTime, AuthOutcome)
                VALUES (@id, @time, @outcome)";

            _dbHelper.ExecuteNonQuery(query,
                new SqliteParameter("@id", userID),
                new SqliteParameter("@time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new SqliteParameter("@outcome", outcome));
        }
    }
}