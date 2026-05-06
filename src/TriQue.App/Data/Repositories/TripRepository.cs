using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using TriQue.Enums;
using TriQue.Helpers;
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

        public List<Trip> GetByDriverID(int driverID)
        {
            var trips = new List<Trip>();

            string query = @"
                SELECT TripID, DriverID, RouteID, StatusID,
                       ActualEarnings, StartTime, EndTime
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
                    StartTime = Convert.ToDateTime(reader["StartTime"]),
                    EndTime = reader["EndTime"] == DBNull.Value
                                        ? null
                                        : Convert.ToDateTime(reader["EndTime"])
                });
            }

            return trips;
        }

        public double GetEarningsProgress(int driverID)
        {
            string query = @"
                SELECT IFNULL(SUM(ActualEarnings), 0)
                FROM Trip
                WHERE DriverID = @driverID";

            using var reader = _dbHelper.ExecuteReader(
                query,
                new SqliteParameter("@driverID", driverID)
            );

            if (!reader.Read()) return 0;

            return Convert.ToDouble(reader[0]);
        }

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

        public int GetTodayTrips(int driverID)
        {
            string query = @"
                SELECT COUNT(*)
                FROM Trip
                WHERE DriverID = @driverID
                AND DATE(StartTime) = @today";

            return Convert.ToInt32(_dbHelper.ExecuteScalar(
                query,
                new SqliteParameter("@driverID", driverID),
                new SqliteParameter("@today", DateTime.Now.ToString("yyyy-MM-dd"))
            ));
        }

        public (double fastest, double slowest) GetTripSpeedStats(int driverID)
        {
            string query = @"
                SELECT 
                    MIN((julianday(EndTime)-julianday(StartTime))*1440) AS Fastest,
                    MAX((julianday(EndTime)-julianday(StartTime))*1440) AS Slowest
                FROM Trip
                WHERE DriverID = @driverID
                AND EndTime IS NOT NULL";

            using var reader = _dbHelper.ExecuteReader(
                query,
                new SqliteParameter("@driverID", driverID)
            );

            if (!reader.Read()) return (0, 0);

            double fastest = reader.IsDBNull(0) ? 0 : Convert.ToDouble(reader["Fastest"]);
            double slowest = reader.IsDBNull(1) ? 0 : Convert.ToDouble(reader["Slowest"]);

            return (fastest, slowest);
        }

        public int? GetActiveTripID(int driverID)
        {
            string query = @"
                SELECT TripID FROM Trip
                WHERE DriverID = @driverID
                  AND StatusID = 2
                  AND EndTime IS NULL
                ORDER BY TripID DESC
                LIMIT 1";

            var result = _dbHelper.ExecuteScalar(
                query,
                new SqliteParameter("@driverID", driverID)
            );

            return result == null ? null : Convert.ToInt32(result);
        }

        public void StartTrip(int driverID, int routeID)
        {
            string query = @"
                INSERT INTO Trip (DriverID, RouteID, StatusID, ActualEarnings, StartTime)
                    VALUES (@driverID, @routeID, 2, 0, @startTime)";

            _dbHelper.ExecuteNonQuery(
                query,
                new SqliteParameter("@driverID", driverID),
                new SqliteParameter("@routeID", routeID),
                new SqliteParameter("@startTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            );
        }

        public void EndTrip(int tripID, double fare)
        {
            string query = @"
                UPDATE Trip
                SET EndTime = @endTime,
                    StatusID = 3,
                    ActualEarnings = @fare
                WHERE TripID = @tripID";

                    _dbHelper.ExecuteNonQuery(
                        query,
                        new SqliteParameter("@endTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        new SqliteParameter("@fare", fare),
                        new SqliteParameter("@tripID", tripID)
                    );
                }

        public DataTable GetTripHistory(int driverID)
        {
            string query = @"
                SELECT 
                    r.RouteName AS Route,
                    '₱ ' || t.ActualEarnings AS Earnings,
                    t.StartTime AS Date
                FROM Trip t
                INNER JOIN Route r ON t.RouteID = r.RouteID
                WHERE t.DriverID = @driverID
                AND t.EndTime IS NOT NULL
                AND t.StatusID = 3
                ORDER BY t.StartTime DESC
                LIMIT 10;
            ";

            using var conn = _dbHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@driverID", driverID);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            return dt;
        }

         // for generating reports
        public DataTable GetTripSummary(DateTime? from, DateTime? to, int? routeID, int? driverID)
        {
            string query = @"
                SELECT 
                    t.StartTime AS Date,
                    u.FirstName || ' ' || u.LastName AS Driver,
                    d.BodyNumber AS [Body No],
                    r.RouteName AS Route,
                    CASE t.StatusID 
                        WHEN 1 THEN 'Waiting'
                        WHEN 2 THEN 'On Trip'
                        WHEN 3 THEN 'Completed'
                    END AS Status,
                    '₱ ' || t.ActualEarnings AS Earnings,
                    CASE 
                        WHEN t.EndTime IS NOT NULL 
                        THEN ROUND((julianday(t.EndTime) - julianday(t.StartTime)) * 1440, 0) || ' min'
                        ELSE '-'
                    END AS Duration
                FROM Trip t
                JOIN Driver d ON t.DriverID = d.DriverID
                JOIN User u ON d.UserID = u.UserID
                JOIN Route r ON t.RouteID = r.RouteID
                WHERE 1=1
                AND (@from IS NULL OR DATE(t.StartTime) >= @from)
                AND (@to IS NULL OR DATE(t.StartTime) <= @to)
                AND (@routeID IS NULL OR t.RouteID = @routeID)
                AND (@driverID IS NULL OR t.DriverID = @driverID)
                ORDER BY t.StartTime DESC";

            using var conn = _dbHelper.GetConnection();
            conn.Open();
            using var cmd = new SqliteCommand(query, conn);

            cmd.Parameters.AddWithValue("@from", from == null ? DBNull.Value : from.Value.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@to", to == null ? DBNull.Value : to.Value.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@routeID", routeID == null ? DBNull.Value : routeID);
            cmd.Parameters.AddWithValue("@driverID", driverID == null ? DBNull.Value : driverID);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            return dt;
        }

        // panels in generating reports
        public (int totalTrips, double totalEarnings, string mostActive, string leastActive, double fastest, double slowest)
            GetReportStats(DateTime? from, DateTime? to, int? routeID, int? driverID)
        {
            string fromStr = from == null ? null : from.Value.ToString("yyyy-MM-dd");
            string toStr = to == null ? null : to.Value.ToString("yyyy-MM-dd");

            // total trips and earnings
            string statsQuery = @"
                SELECT COUNT(*), IFNULL(SUM(ActualEarnings), 0)
                FROM Trip t
                WHERE 1=1
                AND (@from IS NULL OR DATE(t.StartTime) >= @from)
                AND (@to IS NULL OR DATE(t.StartTime) <= @to)
                AND (@routeID IS NULL OR t.RouteID = @routeID)
                AND (@driverID IS NULL OR t.DriverID = @driverID)";

            using var conn = _dbHelper.GetConnection();
            conn.Open();

            using var statsCmd = new SqliteCommand(statsQuery, conn);
            statsCmd.Parameters.AddWithValue("@from", fromStr ?? (object)DBNull.Value);
            statsCmd.Parameters.AddWithValue("@to", toStr ?? (object)DBNull.Value);
            statsCmd.Parameters.AddWithValue("@routeID", routeID ?? (object)DBNull.Value);
            statsCmd.Parameters.AddWithValue("@driverID", driverID ?? (object)DBNull.Value);

            int totalTrips = 0;
            double totalEarnings = 0;

            using (var r = statsCmd.ExecuteReader())
            {
                if (r.Read())
                {
                    totalTrips = Convert.ToInt32(r[0]);
                    totalEarnings = Convert.ToDouble(r[1]);
                }
            }

            // most and least active driver
            string activeQuery = @"
                SELECT u.FirstName || ' ' || u.LastName, COUNT(*) as TripCount
                FROM Trip t
                JOIN Driver d ON t.DriverID = d.DriverID
                JOIN User u ON d.UserID = u.UserID
                WHERE 1=1
                AND (@from IS NULL OR DATE(t.StartTime) >= @from)
                AND (@to IS NULL OR DATE(t.StartTime) <= @to)
                AND (@routeID IS NULL OR t.RouteID = @routeID)
                AND (@driverID IS NULL OR t.DriverID = @driverID)
                GROUP BY t.DriverID
                ORDER BY TripCount {0}
                LIMIT 1";

            string mostActive = "-";
            string leastActive = "-";

            using (var mostCmd = new SqliteCommand(string.Format(activeQuery, "DESC"), conn))
            {
                mostCmd.Parameters.AddWithValue("@from", fromStr ?? (object)DBNull.Value);
                mostCmd.Parameters.AddWithValue("@to", toStr ?? (object)DBNull.Value);
                mostCmd.Parameters.AddWithValue("@routeID", routeID ?? (object)DBNull.Value);
                mostCmd.Parameters.AddWithValue("@driverID", driverID ?? (object)DBNull.Value);
                using var r = mostCmd.ExecuteReader();
                if (r.Read()) mostActive = r[0].ToString();
            }

            using (var leastCmd = new SqliteCommand(string.Format(activeQuery, "ASC"), conn))
            {
                leastCmd.Parameters.AddWithValue("@from", fromStr ?? (object)DBNull.Value);
                leastCmd.Parameters.AddWithValue("@to", toStr ?? (object)DBNull.Value);
                leastCmd.Parameters.AddWithValue("@routeID", routeID ?? (object)DBNull.Value);
                leastCmd.Parameters.AddWithValue("@driverID", driverID ?? (object)DBNull.Value);
                using var r = leastCmd.ExecuteReader();
                if (r.Read()) leastActive = r[0].ToString();
            }

            // fastest and slowest
            string speedQuery = @"
                SELECT 
                    MIN((julianday(EndTime)-julianday(StartTime))*1440),
                    MAX((julianday(EndTime)-julianday(StartTime))*1440)
                FROM Trip t
                WHERE EndTime IS NOT NULL
                AND (@from IS NULL OR DATE(t.StartTime) >= @from)
                AND (@to IS NULL OR DATE(t.StartTime) <= @to)
                AND (@routeID IS NULL OR t.RouteID = @routeID)
                AND (@driverID IS NULL OR t.DriverID = @driverID)";

            double fastest = 0;
            double slowest = 0;

            using (var speedCmd = new SqliteCommand(speedQuery, conn))
            {
                speedCmd.Parameters.AddWithValue("@from", fromStr ?? (object)DBNull.Value);
                speedCmd.Parameters.AddWithValue("@to", toStr ?? (object)DBNull.Value);
                speedCmd.Parameters.AddWithValue("@routeID", routeID ?? (object)DBNull.Value);
                speedCmd.Parameters.AddWithValue("@driverID", driverID ?? (object)DBNull.Value);
                using var r = speedCmd.ExecuteReader();
                if (r.Read())
                {
                    fastest = r.IsDBNull(0) ? 0 : Convert.ToDouble(r[0]);
                    slowest = r.IsDBNull(1) ? 0 : Convert.ToDouble(r[1]);
                }
            }

            return (totalTrips, totalEarnings, mostActive, leastActive, fastest, slowest);
        }
    }
}