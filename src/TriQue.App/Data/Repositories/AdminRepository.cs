using Microsoft.Data.Sqlite;
using System.Data;
using TriQue.Enums;
using TriQue.Helpers;
using TriQue.Models;

namespace TriQue.Data.Repositories
{
    public class AdminRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public AdminRepository()
        {
            _dbHelper = new DatabaseHelper();
        }

        public Admin? GetByUserID(int userID)
        {
            string query = @"
                SELECT AdminID, UserID, LevelID
                FROM Admin
                WHERE UserID = @userID
                LIMIT 1";

            using var reader = _dbHelper.ExecuteReader(
                query,
                new SqliteParameter("@userID", userID)
            );

            if (!reader.Read()) return null;

            return new Admin
            {
                AdminID = Convert.ToInt32(reader["AdminID"]),
                UserID = Convert.ToInt32(reader["UserID"]),
                Level = (AdminLevel)Convert.ToInt32(reader["LevelID"])
            };
        }

       
        public AdminLevel GetAdminLevel(int userID)
        {
            string query = @"
                SELECT a.LevelID FROM Admin a
                JOIN User u ON a.UserID = u.UserID
                WHERE u.UserID = @userID
            ";

            var result = _dbHelper.ExecuteScalar(query,
                new SqliteParameter("@userID", userID));

            return result != null
                ? (AdminLevel)(long)result
                : AdminLevel.Staff; 
        }

        public DataTable GetAllAdmins()
        {
            string query = @"
                SELECT 
                    u.FirstName || ' ' || u.LastName AS [Admin Name],
                    al.LevelName AS [Authorization Level],
                    u.PhoneNumber AS [Contact Number]
                FROM Admin a
                JOIN User u ON a.UserID = u.UserID
                JOIN AdminLevel al ON a.LevelID = al.LevelID
                ORDER BY a.LevelID ASC";

            using var conn = _dbHelper.GetConnection();
            conn.Open();

            using var cmd = new SqliteCommand(query, conn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            return dt;
        }

        public (string FullName, string PhoneNumber, string LevelName)? GetAdminSettings(int userID)
        {
            string query = @"
                SELECT 
                    u.FirstName || ' ' || u.LastName AS FullName,
                    u.PhoneNumber,
                    al.LevelName
                FROM Admin a
                JOIN User u ON a.UserID = u.UserID
                JOIN AdminLevel al ON a.LevelID = al.LevelID
                WHERE a.UserID = @userID
                LIMIT 1";

            using var reader = _dbHelper.ExecuteReader(
                query,
                new SqliteParameter("@userID", userID)
            );

            if (!reader.Read()) return null;

            return (
                FullName: reader["FullName"].ToString() ?? "",
                PhoneNumber: reader["PhoneNumber"].ToString() ?? "",
                LevelName: reader["LevelName"].ToString() ?? ""
            );
        }
    }
}
