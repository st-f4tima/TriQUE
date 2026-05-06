using Microsoft.Data.Sqlite;
using System;
using System.Data;
using TriQue.Enums;
using TriQue.Helpers;
using TriQue.Models;
using TriQue.DTOs;

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

        public int GetAdminLevel(int userID)
        {
            string query = @"
                SELECT a.LevelID 
                FROM Admin a 
                WHERE a.UserID = @id 
                LIMIT 1";

            var result = _dbHelper.ExecuteScalar(query, new SqliteParameter("@id", userID));
            return result == null || result == DBNull.Value ? 0 : Convert.ToInt32(result);
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

        public void InsertLogoutLog(int userID)
        {
            string query = @"
                UPDATE AuthenticationLog
                SET LogoutTime = @time
                WHERE LogID = (
                    SELECT LogID FROM AuthenticationLog
                    WHERE UserID = @id
                    AND LogoutTime IS NULL
                    AND AuthOutcome = 'Success'
                    ORDER BY LoginTime DESC
                    LIMIT 1
                )";

            _dbHelper.ExecuteNonQuery(query,
                new SqliteParameter("@time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new SqliteParameter("@id", userID));
        }

        public List<UserListItem> GetAllUsers(string search = "")
        {
            string query = @"
                SELECT
                u.UserID,
                u.FirstName || ' ' || u.LastName AS FullName,
                u.PhoneNumber,
                r.RoleName,
                ro.RouteName AS AssignedRoute,
                CASE WHEN ds.StatusName IS NULL THEN 'Active' ELSE ds.StatusName END AS Status
                FROM User u
                JOIN UserRole r ON u.RoleID = r.RoleID
                LEFT JOIN Driver d ON d.UserID = u.UserID
                LEFT JOIN Route ro ON ro.AssignedGroup = d.GroupID
                LEFT JOIN DriverStatus ds ON ds.StatusID = d.StatusID
                WHERE (u.FirstName || ' ' || u.LastName) LIKE @search
                OR u.Username LIKE @search
                ORDER BY u.UserID";

            var param = new[] { new SqliteParameter("@search", $"%{search}%") };
            using var reader = _dbHelper.ExecuteReader(query, param);

            var list = new List<UserListItem>();
            while (reader.Read())
            {
                list.Add(new UserListItem
                {
                    UserID = Convert.ToInt32(reader["UserID"]),
                    FullName = reader["FullName"].ToString() ?? "",
                    PhoneNumber = reader["PhoneNumber"].ToString() ?? "",
                    RoleName = reader["RoleName"].ToString() ?? "",
                    AssignedRoute = reader["AssignedRoute"] == DBNull.Value ? "—" : reader["AssignedRoute"].ToString()!,
                    Status = reader["Status"].ToString() ?? "Active"
                });
            }
            return list;
        }

        public UserDetailItem? GetUserDetail(int userID)
        {
            string query = @"
                SELECT
                u.UserID,
                u.FirstName || ' ' || u.LastName AS FullName,
                u.FirstName, u.LastName,
                u.PhoneNumber,
                r.RoleName,
                u.RoleID,
                d.BodyNumber,
                ro.RouteName AS AssignedRoute,
                ro.RouteID,
                COALESCE(ds.StatusName, 'Active') AS Status
                FROM User u
                JOIN UserRole r ON u.RoleID = r.RoleID
                LEFT JOIN Driver d ON d.UserID = u.UserID
                LEFT JOIN Route ro ON ro.AssignedGroup = d.GroupID
                LEFT JOIN DriverStatus ds ON ds.StatusID = d.StatusID
                WHERE u.UserID = @id LIMIT 1";

            var param = new[] { new SqliteParameter("@id", userID) };
            using var reader = _dbHelper.ExecuteReader(query, param);
            if (!reader.Read()) return null;

            return new UserDetailItem
            {
                UserID = Convert.ToInt32(reader["UserID"]),
                FirstName = reader["FirstName"].ToString() ?? "",
                LastName = reader["LastName"].ToString() ?? "",
                FullName = reader["FullName"].ToString() ?? "",
                PhoneNumber = reader["PhoneNumber"].ToString() ?? "",
                RoleName = reader["RoleName"].ToString() ?? "",
                RoleID = Convert.ToInt32(reader["RoleID"]),
                BodyNumber = reader["BodyNumber"] == DBNull.Value ? "" : reader["BodyNumber"].ToString()!,
                AssignedRoute = reader["AssignedRoute"] == DBNull.Value ? "—" : reader["AssignedRoute"].ToString()!,
                RouteID = reader["RouteID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["RouteID"]),
                Status = reader["Status"].ToString() ?? "Active"
            };
        }

        public List<RouteItem> GetAllRoutes()
        {
            string query = "SELECT RouteID, RouteName FROM Route ORDER BY RouteName";
            using var reader = _dbHelper.ExecuteReader(query);
            var list = new List<RouteItem>();
            while (reader.Read())
                list.Add(new RouteItem
                {
                    RouteID = Convert.ToInt32(reader["RouteID"]),
                    RouteName = reader["RouteName"].ToString() ?? ""
                });
            return list;
        }

        public CreatedUserDTO AddUser(string firstName, string lastName, string phone, int roleID, int routeID, int levelID = 3)
        {
            string tempPassword = PasswordHelper.GenerateTempPassword();
            string hashedPassword = PasswordHelper.Hash(tempPassword);

            string username = (firstName.ToLower() + phone[^4..]).Replace(" ", "");

            string insertUser = @"
                INSERT INTO User (Username, PasswordHash, FirstName, LastName, PhoneNumber, RoleID, IsTemporaryPassword)
                VALUES (@user, @pass, @fn, @ln, @phone, @role, 1);
                SELECT last_insert_rowid();";

            var newUserID = Convert.ToInt32(_dbHelper.ExecuteScalar(insertUser,
                new SqliteParameter("@user", username),
                new SqliteParameter("@pass", hashedPassword),
                new SqliteParameter("@fn", firstName),
                new SqliteParameter("@ln", lastName),
                new SqliteParameter("@phone", phone),
                new SqliteParameter("@role", roleID)));


            if (roleID == 1) // Driver
            {
                // find group from route
                string groupQuery = "SELECT AssignedGroup FROM Route WHERE RouteID = @rid LIMIT 1";
                var groupID = Convert.ToInt32(_dbHelper.ExecuteScalar(groupQuery,
                    new SqliteParameter("@rid", routeID)));

                // get next body number
                string bodyQuery = "SELECT COUNT(*) FROM Driver";
                int count = Convert.ToInt32(_dbHelper.ExecuteScalar(bodyQuery)) + 1;
                string bodyNumber = $"TN-{count:D3}";

                string insertDriver = @"
                    INSERT INTO Driver (UserID, GroupID, StatusID, BodyNumber)
                    VALUES (@uid, @gid, 1, @body)";

                _dbHelper.ExecuteNonQuery(insertDriver,
                    new SqliteParameter("@uid", newUserID),
                    new SqliteParameter("@gid", groupID),
                    new SqliteParameter("@body", bodyNumber));
            }
            else if (roleID == 2) // Admin
            {
                string insertAdmin = @"
                    INSERT INTO Admin (UserID, LevelID) VALUES (@uid, @lvl)";

                _dbHelper.ExecuteNonQuery(insertAdmin,
                    new SqliteParameter("@uid", newUserID),
                    new SqliteParameter("@lvl", levelID));
            }

            return new CreatedUserDTO
            {
                Username = username,
                TempPassword = tempPassword
            };
        }

        public void UpdateUser(int userID, string fullName, string phone, int roleID, int routeID)
        {
            string[] parts = fullName.Trim().Split(' ', 2);
            string firstName = parts[0];
            string lastName = parts.Length > 1 ? parts[1] : "";

            string query = @"
                UPDATE User
                SET FirstName = @fn, LastName = @ln, PhoneNumber = @phone, RoleID = @role
                WHERE UserID = @id";

            _dbHelper.ExecuteNonQuery(query,
                new SqliteParameter("@fn", firstName),
                new SqliteParameter("@ln", lastName),
                new SqliteParameter("@phone", phone),
                new SqliteParameter("@role", roleID),
                new SqliteParameter("@id", userID));

            // update driver group if applicable
            if (roleID == 1)
            {
                string groupQuery = "SELECT AssignedGroup FROM Route WHERE RouteID = @rid LIMIT 1";
                
                var groupID = Convert.ToInt32(_dbHelper.ExecuteScalar(groupQuery,
                    new SqliteParameter("@rid", routeID)));

                string driverQuery = @"
                UPDATE Driver SET GroupID = @gid WHERE UserID = @uid";

                _dbHelper.ExecuteNonQuery(driverQuery,
                    new SqliteParameter("@gid", groupID),
                    new SqliteParameter("@uid", userID));
            }
        }

        public void DeleteUser(int userID)
        {
            _dbHelper.ExecuteNonQuery("DELETE FROM Driver WHERE UserID = @id", new SqliteParameter("@id", userID));
            _dbHelper.ExecuteNonQuery("DELETE FROM Admin  WHERE UserID = @id", new SqliteParameter("@id", userID));
            _dbHelper.ExecuteNonQuery("DELETE FROM User   WHERE UserID = @id", new SqliteParameter("@id", userID));
        }

        public void SetNewPassword(int userID, string newPassword)
        {
            string hashed = PasswordHelper.Hash(newPassword);

            string query = @"
                UPDATE User
                SET PasswordHash = @pass, IsTemporaryPassword = 0
                WHERE UserID = @id";

            _dbHelper.ExecuteNonQuery(query,
                new SqliteParameter("@pass", hashed),
                new SqliteParameter("@id", userID));
        }

        public bool IsTemporaryPassword(int userID)
        {
            var result = _dbHelper.ExecuteScalar(
                "SELECT IsTemporaryPassword FROM User WHERE UserID = @id",
                new SqliteParameter("@id", userID));
            return Convert.ToInt32(result) == 1;
        }

    }
}