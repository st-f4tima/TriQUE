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

        // Total trips today
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

        public DataTable GetQueueByRouteID(int routeID)
        {
            string query = @"
                SELECT 
                CASE 
                WHEN qe.Position IS NOT NULL THEN CAST(qe.Position AS TEXT)
                ELSE '-'
                END AS Ranking,
                d.BodyNumber,
                u.FirstName || ' ' || u.LastName AS DriverName,
                ds.StatusName AS TripStatus,
                d.DriverID
                FROM Driver d
                JOIN User u ON d.UserID = u.UserID
                JOIN DriverStatus ds ON d.StatusID = ds.StatusID
                JOIN DriverGroup dg ON d.GroupID = dg.GroupID
                JOIN Route r ON r.AssignedGroup = dg.GroupID
                LEFT JOIN Queue q ON q.RouteID = r.RouteID
                LEFT JOIN QueueEntry qe ON qe.QueueID = q.QueueID 
                AND qe.DriverID = d.DriverID
                WHERE r.RouteID = @routeID
                ORDER BY 
                CASE WHEN qe.Position IS NULL THEN 1 ELSE 0 END,
                qe.Position
        ";

            using var reader = _dbHelper.ExecuteReader(query,
                new SqliteParameter("@routeID", routeID));

            var table = new DataTable();
            table.Columns.Add("Ranking", typeof(string));
            table.Columns.Add("BodyNumber", typeof(string));
            table.Columns.Add("DriverName", typeof(string));
            table.Columns.Add("TripStatus", typeof(string));
            table.Columns.Add("DriverID", typeof(int));

            while (reader.Read())
            {
                table.Rows.Add(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetInt32(4)
                );
            }

            return table;
        }

        public void UpdateDriverStatus(int driverID, int statusID)
        {
            string query = @"
                UPDATE Driver SET StatusID = @statusID
                WHERE DriverID = @driverID
            ";

            _dbHelper.ExecuteNonQuery(query,
                new SqliteParameter("@statusID", statusID),
                new SqliteParameter("@driverID", driverID)
            );
        }

        public void ResetQueue(int routeID)
        {

            string updateStatus = @"
                UPDATE Driver 
                SET StatusID = 3
                WHERE StatusID = 1
                AND DriverID IN (
                    SELECT qe.DriverID 
                    FROM QueueEntry qe
                    JOIN Queue q ON qe.QueueID = q.QueueID
                    WHERE q.RouteID = @routeID
                )
                ";

            _dbHelper.ExecuteNonQuery(updateStatus,
                new SqliteParameter("@routeID", routeID)
            );

            string clearQueue = @"
                DELETE FROM QueueEntry
                WHERE QueueID = (
                    SELECT QueueID FROM Queue WHERE RouteID = @routeID
                )
                ";

            _dbHelper.ExecuteNonQuery(clearQueue,
                new SqliteParameter("@routeID", routeID)
            );
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
    }
}
