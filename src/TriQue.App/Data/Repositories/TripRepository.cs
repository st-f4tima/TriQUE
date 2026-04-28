using Microsoft.Data.Sqlite;
using System;
using System.Data;
using TriQue.Data.Database;
using TriQue.Enums;
using TriQue.Models;

namespace TriQue.Data.Repositories
{
    public class TripRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public TripRepository()
        {
            _dbHelper = new DatabaseHelper();
        }

        // list of trips
        public List<Trip> GetByDriverID(int driverID)
        {
            var trips = new List<Trip>();

            string query = @"
                SELECT TripID, DriverID, RouteID, StatusID,
                       ActualEarnings, GoalEarning, StartTime, EndTime
                FROM Trip
                WHERE DriverID = @driverID";

            using var reader = _dbHelper.ExecuteReader(
                query,
                new SqliteParameter("@driverID", driverID)
            );

            while (reader.Read())
            {
                trips.Add(new Trip
                {
                    TripID = Convert.ToInt32(reader["TripID"]),
                    DriverID = Convert.ToInt32(reader["DriverID"]),
                    RouteID = Convert.ToInt32(reader["RouteID"]),
                    Status = (DriverStatus)Convert.ToInt32(reader["StatusID"]),
                    ActualEarnings = Convert.ToDouble(reader["ActualEarnings"]),
                    GoalEarning = Convert.ToDouble(reader["GoalEarning"]),
                    StartTime = Convert.ToDateTime(reader["StartTime"]),
                    EndTime = reader["EndTime"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(reader["EndTime"])
                });
            }

            return trips;
        }

        // progress bar (earnings)
        public (double actual, double goal) GetEarningsProgress(int driverID)
        {
            string query = @"
                SELECT 
                    IFNULL(SUM(ActualEarnings), 0),
                    IFNULL(SUM(GoalEarning), 0)
                FROM Trip
                WHERE DriverID = @driverID";

            using var reader = _dbHelper.ExecuteReader(
                query,
                new SqliteParameter("@driverID", driverID)
            );

            if (!reader.Read())
                return (0, 0);

            return (
                Convert.ToDouble(reader[0]),
                Convert.ToDouble(reader[1])
            );

        }

        // all completed trips
        public int GetCompletedTrips(int driverID)
        {
            string query = @"
                SELECT COUNT(*)
                FROM Trip
                WHERE DriverID = @driverID
                AND StatusID = 3";

            return Convert.ToInt32(_dbHelper.ExecuteScalar(
                query,
                new SqliteParameter("@driverID", driverID)
            ));

        }

        // today trips
        public int GetTodayTrips(int driverID)
        {
            string query = @"
                SELECT COUNT(*)
                FROM Trip
                WHERE DriverID = @driverID
                AND DATE(StartTime) = @today";

            var today = DateTime.Now.ToString("yyyy-MM-dd");

            return Convert.ToInt32(_dbHelper.ExecuteScalar(
                query,
                new SqliteParameter("@driverID", driverID),
                new SqliteParameter("@today", today)
            ));

        }

        // fastest and slowest trips
        public (double fastest, double slowest) GetTripSpeedStats(int driverID)
        {
            string query = @"
                SELECT 
                    MIN((julianday(EndTime)-julianday(StartTime))*1440) AS Fastest,
                    MAX((julianday(EndTime)-julianday(StartTime))*1440) AS Slowest
                FROM Trip
                WHERE DriverID = @driverId
                AND EndTime IS NOT NULL";

            using var conn = _dbHelper.GetConnection();
            conn.Open();

            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@driverId", driverID);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return (0, 0);

            double fastest = reader.IsDBNull(0) ? 0 : Convert.ToDouble(reader["Fastest"]);
            double slowest = reader.IsDBNull(1) ? 0 : Convert.ToDouble(reader["Slowest"]);

            return (fastest, slowest);
        }

        public DataTable GetTripGrid(int driverID)
        {
            string query = @"
                SELECT 
                    qe.Position AS Position,
                    r.RouteName AS Route,
                    t.StartTime AS Date
                FROM Trip t
                JOIN Route r ON t.RouteID = r.RouteID
                JOIN QueueEntry qe ON qe.DriverID = t.DriverID
                WHERE t.DriverID = @driverID
                ORDER BY t.StartTime DESC";

            using var conn = _dbHelper.GetConnection();
            conn.Open();

            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@driverID", driverID);

            using var reader = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader); // ✅ THIS replaces DataAdapter

            return dt;
        }

    }
}

    
