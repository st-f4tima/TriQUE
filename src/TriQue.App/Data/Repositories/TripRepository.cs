using Microsoft.Data.Sqlite;
using System;
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
    }
}

    
