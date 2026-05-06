using Microsoft.Data.Sqlite;
using System;
using TriQue.DTOs;
using TriQue.Enums;
using TriQue.Helpers;
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

        public Driver? GetByUserID(int userID)
        {
            string query = @"
                SELECT DriverID, UserID, GroupID, StatusID, BodyNumber, GoalEarnings
                FROM Driver
                WHERE UserID = @userID
                LIMIT 1";

            using var reader = _dbHelper.ExecuteReader(
                query,
                new SqliteParameter("@userID", userID)
            );

            if (!reader.Read()) return null;

            return new Driver
            {
                DriverID = Convert.ToInt32(reader["DriverID"]),
                UserID = Convert.ToInt32(reader["UserID"]),
                GroupID = Convert.ToInt32(reader["GroupID"]),
                BodyNumber = reader["BodyNumber"].ToString(),
                Status = (DriverStatus)Convert.ToInt32(reader["StatusID"]),
                GoalEarnings = Convert.ToDouble(reader["GoalEarnings"])
            };
        }

        public Driver? GetByDriverID(int driverID)
        {
            string query = @"
                SELECT DriverID, UserID, GroupID, StatusID, BodyNumber, GoalEarnings
                FROM Driver
                WHERE DriverID = @driverID";

            using var reader = _dbHelper.ExecuteReader(
                query,
                new SqliteParameter("@driverID", driverID)
            );

            if (!reader.Read()) return null;

            return new Driver
            {
                DriverID = Convert.ToInt32(reader["DriverID"]),
                UserID = Convert.ToInt32(reader["UserID"]),
                GroupID = Convert.ToInt32(reader["GroupID"]),
                Status = (DriverStatus)Convert.ToInt32(reader["StatusID"]),
                BodyNumber = reader["BodyNumber"].ToString(),
                GoalEarnings = Convert.ToDouble(reader["GoalEarnings"])
            };
        }

        public void UpdateStatus(int driverId, int statusId)
        {
            string query = @"
                UPDATE Driver
                SET StatusID = $statusId
                WHERE DriverID = $driverId;
            ";

            _dbHelper.ExecuteNonQuery(
                query,
                new SqliteParameter("$statusId", statusId),
                new SqliteParameter("$driverId", driverId)
            );
        }

        public (string FullName, string BodyNumber, string PhoneNumber, string RouteName, string GroupName, string StatusName)? GetDriverSettings(int userID)
        {
            string query = @"
                SELECT 
                    u.FirstName || ' ' || u.LastName AS FullName,
                    d.BodyNumber,
                    u.PhoneNumber,
                    r.RouteName,
                    g.GroupName,
                    ds.StatusName
                FROM Driver d
                JOIN User u ON d.UserID = u.UserID
                JOIN DriverGroup g ON d.GroupID = g.GroupID
                JOIN DriverStatus ds ON d.StatusID = ds.StatusID
                LEFT JOIN Route r ON r.AssignedGroup = d.GroupID
                WHERE d.UserID = @userID
                LIMIT 1";

            using var reader = _dbHelper.ExecuteReader(
                query,
                new SqliteParameter("@userID", userID)
            );

            if (!reader.Read()) return null;

            return (
                FullName: reader["FullName"].ToString() ?? "",
                BodyNumber: reader["BodyNumber"].ToString() ?? "",
                PhoneNumber: reader["PhoneNumber"].ToString() ?? "",
                RouteName: reader["RouteName"].ToString() ?? "No Route Assigned",
                GroupName: reader["GroupName"].ToString() ?? "",
                StatusName: reader["StatusName"].ToString() ?? "Waiting"
            );
        }

        public DriverGroup? GetGroupByID(int groupID)
        {
            string query = "SELECT GroupID, GroupName, RotationDay FROM DriverGroup WHERE GroupID = @groupID";

            using var reader = _dbHelper.ExecuteReader(
                query,
                new SqliteParameter("@groupID", groupID)
            );

            if (!reader.Read()) return null;

            return new DriverGroup
            {
                GroupID = Convert.ToInt32(reader["GroupID"]),
                GroupName = reader["GroupName"].ToString(),
                GroupRotationDay = (RotationDay)Convert.ToInt32(reader["RotationDay"])
            };
        }

        public List<DriverDto> GetAllDrivers()
        {
            var drivers = new List<DriverDto>();

            string query = @"
                SELECT d.DriverID, u.FirstName || ' ' || u.LastName AS FullName
                FROM Driver d
                JOIN User u ON d.UserID = u.UserID
                ORDER BY FullName";

            using var reader = _dbHelper.ExecuteReader(query);

            while (reader.Read())
            {
                drivers.Add(new DriverDto
                {
                    DriverID = Convert.ToInt32(reader["DriverID"]),
                    FullName = reader["FullName"].ToString()
                });
            }

            return drivers;
        }
    }
}