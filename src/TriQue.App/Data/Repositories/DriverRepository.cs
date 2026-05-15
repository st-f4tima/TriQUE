using Microsoft.Data.Sqlite;
using System;
using System.Data;
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

        #region Driver Retrieval
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

        public DriverSettingsDto? GetDriverSettings(int userID)
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

            return new DriverSettingsDto
            {
                FullName = reader["FullName"].ToString() ?? "",
                BodyNumber = reader["BodyNumber"].ToString() ?? "",
                PhoneNumber = reader["PhoneNumber"].ToString() ?? "",
                RouteName = reader["RouteName"].ToString() ?? "No Route Assigned",
                GroupName = reader["GroupName"].ToString() ?? "",
                StatusName = reader["StatusName"].ToString() ?? "Waiting"
            };
        }

        #endregion

        #region Driver Actions
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

        #endregion

        #region Group Queries

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

        public List<DriverGroup> GetAllGroups()
        {
            string query = "SELECT GroupID, GroupName, RotationDay FROM DriverGroup";
            var groups = new List<DriverGroup>();

            using var reader = _dbHelper.ExecuteReader(query);

            while (reader.Read())
            {
                groups.Add(new DriverGroup
                {
                    GroupID = Convert.ToInt32(reader["GroupID"]),
                    GroupName = reader["GroupName"].ToString(),
                    GroupRotationDay = (RotationDay)Convert.ToInt32(reader["RotationDay"])
                });
            }

            return groups;
        }

        #endregion

        #region Chart Data

        // pie graph
        public Dictionary<string, int> GetDriverStatusDistribution()
        {
            string query = @"
                SELECT ds.StatusName, COUNT(d.DriverID) as Total
                FROM Driver d
                JOIN DriverStatus ds ON d.StatusID = ds.StatusID
                GROUP BY d.StatusID
            ";

            var result = new Dictionary<string, int>();

            using var conn = _dbHelper.GetConnection();
            conn.Open();

            using var cmd = new SqliteCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                result[reader.GetString(0)] = reader.GetInt32(1);
            }

            return result;
        }

        // bar graph
        public Dictionary<string, int> GetDriversPerRoute()
        {
            string query = @"
                SELECT r.RouteName, COUNT(d.DriverID) as Total
                FROM Driver d
                JOIN DriverGroup dg ON d.GroupID = dg.GroupID
                JOIN Route r ON r.AssignedGroup = dg.GroupID
                GROUP BY r.RouteID
                ORDER BY r.RouteID
            ";

            var result = new Dictionary<string, int>();

            using var conn = _dbHelper.GetConnection();
            conn.Open();

            using var cmd = new SqliteCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                result[reader.GetString(0)] = reader.GetInt32(1);
            }

            return result;
        }

        #endregion

        #region report

        public List<DriverPerformanceDto> GetDriverPerformance(DateTime? from, DateTime? to, int? routeID, int? driverID)
        {
            var result = new List<DriverPerformanceDto>();

            string query = @"
                SELECT
                    d.DriverID,
                    u.FirstName || ' ' || u.LastName AS Driver,
                    d.BodyNumber AS [Body No.],
                    dg.GroupName AS [Group],
                    COUNT(t.TripID) AS [Total Trips],
                    SUM(CASE WHEN t.StatusID = 3 THEN 1 ELSE 0 END) AS [Completed],
                    IFNULL(SUM(CASE WHEN t.StatusID = 3 THEN t.ActualEarnings ELSE 0 END), 0) AS [Total Earnings],
                    IFNULL(AVG(CASE WHEN t.EndTime IS NOT NULL 
                        THEN (julianday(t.EndTime) - julianday(t.StartTime)) * 1440 
                        ELSE NULL END), 0) AS [Avg Duration],
                    IFNULL(MIN(CASE WHEN t.EndTime IS NOT NULL 
                        THEN (julianday(t.EndTime) - julianday(t.StartTime)) * 1440 
                        ELSE NULL END), 0) AS [Fastest],
                    IFNULL(MAX(CASE WHEN t.EndTime IS NOT NULL 
                        THEN (julianday(t.EndTime) - julianday(t.StartTime)) * 1440 
                        ELSE NULL END), 0) AS [Slowest]
                FROM Driver d
                JOIN User u ON d.UserID = u.UserID
                JOIN DriverGroup dg ON d.GroupID = dg.GroupID
                LEFT JOIN Trip t ON t.DriverID = d.DriverID
                    AND (@from IS NULL OR DATE(t.StartTime) >= @from)
                    AND (@to IS NULL OR DATE(t.StartTime) <= @to)
                    AND (@routeID IS NULL OR t.RouteID = @routeID)  -- ← moved here
                WHERE (@driverID IS NULL OR d.DriverID = @driverID) -- ← only driver filter here
                GROUP BY d.DriverID
                ORDER BY [Completed] DESC";

            using var conn = _dbHelper.GetConnection();
            conn.Open();

            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@from", from == null ? DBNull.Value : from.Value.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@to", to == null ? DBNull.Value : to.Value.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@routeID", routeID == null ? DBNull.Value : routeID);
            cmd.Parameters.AddWithValue("@driverID", driverID == null ? DBNull.Value : driverID);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new DriverPerformanceDto
                {
                    DriverID = Convert.ToInt32(reader["DriverID"]),
                    FullName = reader["Driver"].ToString() ?? "",
                    BodyNumber = reader["Body No."].ToString() ?? "",
                    GroupName = reader["Group"].ToString() ?? "",
                    TotalTrips = Convert.ToInt32(reader["Total Trips"]),
                    CompletedTrips = Convert.ToInt32(reader["Completed"]),
                    TotalEarnings = Convert.ToDouble(reader["Total Earnings"]),
                    AvgDuration = Convert.ToDouble(reader["Avg Duration"]),
                    FastestTrip = Convert.ToDouble(reader["Fastest"]),
                    SlowestTrip = Convert.ToDouble(reader["Slowest"])
                });
            }

            return result;
        }

        public (string topEarner, double topEarnings, string mostTrips, int tripCount, double avgDuration)
         GetDriverPerformanceStats(DateTime? from, DateTime? to)
        {
            string fromStr = from?.ToString("yyyy-MM-dd");
            string toStr = to?.ToString("yyyy-MM-dd");

            using var conn = _dbHelper.GetConnection();
            conn.Open();

            // Top earner
            string earnerQuery = @"
                SELECT u.FirstName || ' ' || u.LastName, IFNULL(SUM(t.ActualEarnings), 0) AS Total
                FROM Driver d
                JOIN User u ON d.UserID = u.UserID
                LEFT JOIN Trip t ON t.DriverID = d.DriverID AND t.StatusID = 3
                    AND (@from IS NULL OR DATE(t.StartTime) >= @from)
                    AND (@to IS NULL OR DATE(t.StartTime) <= @to)
                GROUP BY d.DriverID
                ORDER BY Total DESC LIMIT 1";

            string topEarner = "-";
            double topEarnings = 0;

            using (var cmd = new SqliteCommand(earnerQuery, conn))
            {
                cmd.Parameters.AddWithValue("@from", fromStr ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@to", toStr ?? (object)DBNull.Value);
                using var r = cmd.ExecuteReader();
                if (r.Read()) { topEarner = r[0].ToString()!; topEarnings = Convert.ToDouble(r[1]); }
            }

            // Most trips
            string tripsQuery = @"
                SELECT u.FirstName || ' ' || u.LastName, COUNT(t.TripID) AS Total
                FROM Driver d
                JOIN User u ON d.UserID = u.UserID
                LEFT JOIN Trip t ON t.DriverID = d.DriverID AND t.StatusID = 3
                    AND (@from IS NULL OR DATE(t.StartTime) >= @from)
                    AND (@to IS NULL OR DATE(t.StartTime) <= @to)
                GROUP BY d.DriverID
                ORDER BY Total DESC LIMIT 1";

            string mostTrips = "-";
            int tripCount = 0;

            using (var cmd = new SqliteCommand(tripsQuery, conn))
            {
                cmd.Parameters.AddWithValue("@from", fromStr ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@to", toStr ?? (object)DBNull.Value);
                using var r = cmd.ExecuteReader();
                if (r.Read()) { mostTrips = r[0].ToString()!; tripCount = Convert.ToInt32(r[1]); }
            }

            // Avg duration across all drivers
            string avgQuery = @"
                SELECT ROUND(IFNULL(AVG((julianday(EndTime) - julianday(StartTime)) * 1440), 0), 1)
                FROM Trip
                WHERE EndTime IS NOT NULL AND StatusID = 3
                AND (@from IS NULL OR DATE(StartTime) >= @from)
                AND (@to IS NULL OR DATE(StartTime) <= @to)";

            double avgDuration = 0;

            using (var cmd = new SqliteCommand(avgQuery, conn))
            {
                cmd.Parameters.AddWithValue("@from", fromStr ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@to", toStr ?? (object)DBNull.Value);
                var result = cmd.ExecuteScalar();
                avgDuration = result == null || result == DBNull.Value ? 0 : Convert.ToDouble(result);
            }

            return (topEarner, topEarnings, mostTrips, tripCount, avgDuration);
        }

        #endregion
    }
}