using Microsoft.Data.Sqlite;
using TriQue.DTOs;
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

        #region User Retrieval Queries

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
            UserRole role = (UserRole)roleId;

            User user = role switch
            {
                UserRole.Admin => new Admin(),
                UserRole.Driver => new Driver(),
                _ => throw new Exception($"Invalid RoleID: {roleId}")
            };

            user.UserID = Convert.ToInt32(reader["UserID"]);
            user.Username = reader["Username"].ToString() ?? "";
            user.PasswordHash = reader["PasswordHash"].ToString() ?? "";
            user.Role = role;

            return user;
        }

        public List<UserListDto> GetAllUsers(string search = "")
        {
            string query = @"
                SELECT
                    u.UserID,
                    u.FirstName || ' ' || u.LastName AS FullName,
                    u.PhoneNumber,
                    r.RoleName,
                    COALESCE(d.GroupID, 0) AS GroupID,
                    COALESCE(dg.GroupName, '—') AS GroupName,
                    CASE WHEN ds.StatusName IS NULL THEN 'Active' ELSE ds.StatusName END AS Status
                FROM User u
                JOIN UserRole r ON u.RoleID = r.RoleID
                LEFT JOIN Driver d ON d.UserID = u.UserID
                LEFT JOIN DriverGroup dg ON dg.GroupID = d.GroupID
                LEFT JOIN DriverStatus ds ON ds.StatusID = d.StatusID
                WHERE (u.FirstName || ' ' || u.LastName) LIKE @search
                OR u.Username LIKE @search
                ORDER BY u.UserID";

            var param = new[] { new SqliteParameter("@search", $"%{search}%") };
            using var reader = _dbHelper.ExecuteReader(query, param);

            var list = new List<UserListDto>();
            while (reader.Read())
            {
                list.Add(new UserListDto
                {
                    UserID = Convert.ToInt32(reader["UserID"]),
                    FullName = reader["FullName"].ToString() ?? "",
                    PhoneNumber = reader["PhoneNumber"].ToString() ?? "",
                    RoleName = reader["RoleName"].ToString() ?? "",
                    GroupID = Convert.ToInt32(reader["GroupID"]),
                    GroupName = reader["GroupName"].ToString() ?? "—",
                    AssignedRoute = "",
                    Status = reader["Status"].ToString() ?? "Active"
                });
            }
            return list;
        }

        public UserDetailDto? GetUserDetail(int userID)
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
                    COALESCE(d.GroupID, 0) AS GroupID,
                    COALESCE(dg.GroupName, '—') AS GroupName,
                    COALESCE(ds.StatusName, 'Active') AS Status
                FROM User u
                JOIN UserRole r ON u.RoleID = r.RoleID
                LEFT JOIN Driver d ON d.UserID = u.UserID
                LEFT JOIN DriverGroup dg ON dg.GroupID = d.GroupID
                LEFT JOIN DriverStatus ds ON ds.StatusID = d.StatusID
                WHERE u.UserID = @id LIMIT 1";

            var param = new[] { new SqliteParameter("@id", userID) };
            using var reader = _dbHelper.ExecuteReader(query, param);
            if (!reader.Read()) return null;

            return new UserDetailDto
            {
                UserID = Convert.ToInt32(reader["UserID"]),
                FirstName = reader["FirstName"].ToString() ?? "",
                LastName = reader["LastName"].ToString() ?? "",
                FullName = reader["FullName"].ToString() ?? "",
                PhoneNumber = reader["PhoneNumber"].ToString() ?? "",
                RoleName = reader["RoleName"].ToString() ?? "",
                RoleID = Convert.ToInt32(reader["RoleID"]),
                BodyNumber = reader["BodyNumber"] == DBNull.Value ? "" : reader["BodyNumber"].ToString()!,
                GroupID = Convert.ToInt32(reader["GroupID"]),
                GroupName = reader["GroupName"].ToString() ?? "—",
                Status = reader["Status"].ToString() ?? "Active"
            };
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

        #endregion

        #region User Management Queries
        public CreatedUserDto AddUser(string firstName, string lastName, string phone, int roleID, int groupID, int levelID = 3)
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
                string bodyQuery = "SELECT COUNT(*) FROM Driver";
                int count = Convert.ToInt32(_dbHelper.ExecuteScalar(bodyQuery)) + 1;
                string bodyNumber = $"TN-{count:D3}";

                _dbHelper.ExecuteNonQuery(@"
                    INSERT INTO Driver (UserID, GroupID, StatusID, BodyNumber)
                    VALUES (@uid, @gid, 1, @body)",
                    new SqliteParameter("@uid", newUserID),
                    new SqliteParameter("@gid", groupID),
                    new SqliteParameter("@body", bodyNumber));
            }
            else if (roleID == 2) // Admin
            {
                _dbHelper.ExecuteNonQuery(@"
                    INSERT INTO Admin (UserID, LevelID) VALUES (@uid, @lvl)",
                    new SqliteParameter("@uid", newUserID),
                    new SqliteParameter("@lvl", levelID));
            }

            return new CreatedUserDto
            {
                Username = username,
                TempPassword = tempPassword
            };
        }

        public void UpdateUser(int userID, string fullName, string phone, int roleID, int groupID, int levelID)
        {
            string[] parts = fullName.Trim().Split(' ', 2);
            string firstName = parts[0];
            string lastName = parts.Length > 1 ? parts[1] : "";

            // get current role
            var currentRoleResult = _dbHelper.ExecuteScalar(
                "SELECT RoleID FROM User WHERE UserID = @id",
                new SqliteParameter("@id", userID));

            int currentRoleID = Convert.ToInt32(currentRoleResult);

            // update user table
            _dbHelper.ExecuteNonQuery(@"
                UPDATE User
                SET FirstName = @fn, LastName = @ln, PhoneNumber = @phone, RoleID = @role
                WHERE UserID = @id",
                new SqliteParameter("@fn", firstName),
                new SqliteParameter("@ln", lastName),
                new SqliteParameter("@phone", phone),
                new SqliteParameter("@role", roleID),
                new SqliteParameter("@id", userID));

            // handle role change
            if (currentRoleID != roleID)
            {
                // remove from old role table
                if (currentRoleID == 1)
                {
                    _dbHelper.ExecuteNonQuery("DELETE FROM Driver WHERE UserID = @uid",
                        new SqliteParameter("@uid", userID));
                } else if (currentRoleID == 2)
                {
                    _dbHelper.ExecuteNonQuery("DELETE FROM Admin WHERE UserID = @uid",
                        new SqliteParameter("@uid", userID));
                }


                // Insert into new role table
                if (roleID == 1) // now a Driver
                {
                    string bodyQuery = "SELECT COUNT(*) FROM Driver";
                    int count = Convert.ToInt32(_dbHelper.ExecuteScalar(bodyQuery)) + 1;
                    string bodyNumber = $"TN-{count:D3}";

                    _dbHelper.ExecuteNonQuery(@"
                INSERT INTO Driver (UserID, GroupID, StatusID, BodyNumber)
                VALUES (@uid, @gid, 1, @body)",
                        new SqliteParameter("@uid", userID),
                        new SqliteParameter("@gid", groupID),
                        new SqliteParameter("@body", bodyNumber));
                }
                else if (roleID == 2) // now an Admin
                {
                    _dbHelper.ExecuteNonQuery(@"
                INSERT INTO Admin (UserID, LevelID) VALUES (@uid, @lvl)",
                        new SqliteParameter("@uid", userID),
                        new SqliteParameter("@lvl", levelID));
                }
            }
            else
            {
                // same role — just update the relevant table
                if (roleID == 1)
                    _dbHelper.ExecuteNonQuery(
                        "UPDATE Driver SET GroupID = @gid WHERE UserID = @uid",
                        new SqliteParameter("@gid", groupID),
                        new SqliteParameter("@uid", userID));
                else if (roleID == 2)
                    _dbHelper.ExecuteNonQuery(
                        "UPDATE Admin SET LevelID = @lid WHERE UserID = @uid",
                        new SqliteParameter("@lid", levelID),
                        new SqliteParameter("@uid", userID));
            }
        }

        public void DeleteUser(int userID)
        {
            var driverID = _dbHelper.ExecuteScalar(
                "SELECT DriverID FROM Driver WHERE UserID = @id",
                new SqliteParameter("@id", userID));

            if (driverID != null)
            {
                int dID = Convert.ToInt32(driverID);
                _dbHelper.ExecuteNonQuery("DELETE FROM QueueEntry WHERE DriverID = @id",
                    new SqliteParameter("@id", dID));

                _dbHelper.ExecuteNonQuery("DELETE FROM Trip WHERE DriverID = @id",
                    new SqliteParameter("@id", dID));

                _dbHelper.ExecuteNonQuery("DELETE FROM Driver WHERE DriverID = @id",
                    new SqliteParameter("@id", dID));
            }

            _dbHelper.ExecuteNonQuery("DELETE FROM Admin WHERE UserID = @id",
                new SqliteParameter("@id", userID));

            _dbHelper.ExecuteNonQuery("DELETE FROM AuthenticationLog WHERE UserID = @id",
                new SqliteParameter("@id", userID));

            _dbHelper.ExecuteNonQuery("DELETE FROM User WHERE UserID = @id",
                new SqliteParameter("@id", userID));
        }

        #endregion

        #region Authentication Queries

        public bool IsTemporaryPassword(int userID)
        {
            var result = _dbHelper.ExecuteScalar(
                "SELECT IsTemporaryPassword FROM User WHERE UserID = @id",
                new SqliteParameter("@id", userID));
            return Convert.ToInt32(result) == 1;
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

        public void InsertAuthLog(AuthenticationLog log)
        {
            string query = @"
                INSERT INTO AuthenticationLog (UserID, LoginTime, AuthOutcome)
                VALUES (@id, @time, @outcome)";

            _dbHelper.ExecuteNonQuery(query,
                 new SqliteParameter("@id", log.UserID),
                 new SqliteParameter("@time", log.LoginTime.ToString("yyyy-MM-dd HH:mm:ss")),
                 new SqliteParameter("@outcome", log.AuthOutcome));
        }

        public void InsertLogoutLog(AuthenticationLog log)
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
                new SqliteParameter("@time", log.LogoutTime.ToString("yyyy-MM-dd HH:mm:ss")),
                new SqliteParameter("@id", log.UserID));
        }

        #endregion

    }
}