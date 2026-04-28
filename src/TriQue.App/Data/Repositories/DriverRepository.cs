using Microsoft.Data.Sqlite;
using System;
using TriQue.Data.Database;
using TriQue.Enums;
using TriQue.Models;

namespace TriQue.Data.Repositories
{
    public class DriverRepository
    {
        private readonly DatabaseHelper _dbHelper;
        public DriverRepository()
        {
            _dbHelper = new DatabaseHelper();
        }

        public Driver? GetByUserId(int userID)
        {
            string query = @"
                SELECT DriverID, UserID, GroupID, StatusID, BodyNumber
                FROM Driver
                WHERE UserID = @userID
                LIMIT 1";

            using var reader = _dbHelper.ExecuteReader(query,
                new SqliteParameter("@userID", userID));

            if (!reader.Read()) return null;

            return new Driver
            {
                DriverID = Convert.ToInt32(reader["DriverID"]),
                UserID = Convert.ToInt32(reader["UserID"]),
                GroupID = Convert.ToInt32(reader["GroupID"]),
                BodyNumber = reader["BodyNumber"].ToString(),
                Status = (DriverStatus)Convert.ToInt32(reader["StatusID"])
            };


        }
    }
}
