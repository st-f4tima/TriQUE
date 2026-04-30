using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
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

        public void EndTrip(int driverID)
        {
            string query = @"
                UPDATE Trip
                SET    EndTime  = @endTime,
                       StatusID = 3
                WHERE  DriverID = @driverID
                  AND  StatusID = 2
                  AND  EndTime  IS NULL
                  AND  TripID = (
                      SELECT TripID FROM Trip
                      WHERE  DriverID = @driverID
                        AND  StatusID = 2
                        AND  EndTime  IS NULL
                      ORDER  BY TripID DESC
                      LIMIT  1
                  )";

            _dbHelper.ExecuteNonQuery(
                query,
                new SqliteParameter("@endTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new SqliteParameter("@driverID", driverID)
            );
        }
    }
}