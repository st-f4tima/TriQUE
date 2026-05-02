using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using TriQue.Helpers;
using TriQue.Models;
using TriQue.Enums;

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

        // ── Total Trips Today: route name with most trips started today ──────────
        public string GetTotalTripsTodayRoute()
        {
            string query = @"
        SELECT r.RouteName, COUNT(t.TripID) as Total
        FROM Trip t
        JOIN Route r ON t.RouteID = r.RouteID
        WHERE DATE(t.StartTime) = DATE('now', 'localtime')
        GROUP BY t.RouteID
        ORDER BY Total DESC
        LIMIT 1
    ";

            using var conn = _dbHelper.GetConnection();
            conn.Open();

            using var cmd = new SqliteCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            return reader.Read() ? reader.GetString(0) : "No Trips Yet";
        }

        // highest trip
        public (string routeName, int count) GetHighestTripsRoute()
        {
            string query = @"
                SELECT r.RouteName, COUNT(t.TripID) as Total
                FROM Trip t
                JOIN Route r ON t.RouteID = r.RouteID
                GROUP BY t.RouteID
                ORDER BY Total DESC
                LIMIT 1
            ";

            using var conn = _dbHelper.GetConnection();
            conn.Open();

            using var cmd = new SqliteCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            return reader.Read()
                ? (reader.GetString(0), reader.GetInt32(1))
                : ("No Data", 0);
        }

        // Lowest trip
        public (string routeName, int count) GetLowestTripsRoute()
        {
            string query = @"
                SELECT r.RouteName, COUNT(t.TripID) as Total
                FROM Trip t
                JOIN Route r ON t.RouteID = r.RouteID
                GROUP BY t.RouteID
                ORDER BY Total ASC
                LIMIT 1
            ";

            using var conn = _dbHelper.GetConnection();
            conn.Open();

            using var cmd = new SqliteCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            return reader.Read()
                ? (reader.GetString(0), reader.GetInt32(1))
                : ("No Data", 0);
        }
    }
}
