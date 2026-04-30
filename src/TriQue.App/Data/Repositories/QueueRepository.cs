using Microsoft.Data.Sqlite;
using System.Data;
using TriQue.Helpers;

namespace TriQue.Data.Repositories
{
    public class QueueRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public QueueRepository()
        {
            _dbHelper = new DatabaseHelper();
        }

        public int GetQueueIdByRouteId(int routeId)
        {
            string query = @"
                SELECT QueueID 
                FROM Queue 
                WHERE RouteID = $routeId
                LIMIT 1;
            ";

            var result = _dbHelper.ExecuteScalar(
                query,
                new SqliteParameter("$routeId", routeId)
            );

            if (result == null)
                throw new Exception("Queue not found for this route.");

            return Convert.ToInt32(result);
        }

        public bool IsDriverAlreadyInQueue(int queueId, int driverId)
        {
            string query = @"
                SELECT COUNT(*) 
                FROM QueueEntry
                WHERE QueueID = $queueId AND DriverID = $driverId;
            ";

            var result = _dbHelper.ExecuteScalar(
                query,
                new SqliteParameter("$queueId", queueId),
                new SqliteParameter("$driverId", driverId)
            );

            return Convert.ToInt32(result) > 0;
        }


        public int GetNextPosition(int queueId)
        {
            string query = @"
                SELECT IFNULL(MAX(Position), 0) + 1
                FROM QueueEntry
                WHERE QueueID = $queueId;
            ";

            var result = _dbHelper.ExecuteScalar(
                query,
                new SqliteParameter("$queueId", queueId)
            );

            return Convert.ToInt32(result);
        }

        public void AddQueueEntry(int queueId, int driverId, int position)
        {
            string query = @"
                INSERT INTO QueueEntry (QueueID, DriverID, Position, JoinedAt)
                VALUES ($queueId, $driverId, $position, datetime('now'));
            ";

            _dbHelper.ExecuteNonQuery(
                query,
                new SqliteParameter("$queueId", queueId),
                new SqliteParameter("$driverId", driverId),
                new SqliteParameter("$position", position)
            );
        }

        public DataTable GetQueueHistory(int driverID)
        {
            string query = @"
                SELECT 
                    qe.Position AS Position,
                    r.RouteName AS Route,
                    qe.JoinedAt AS JoinedAt
                FROM QueueEntry qe
                JOIN Queue q ON qe.QueueID = q.QueueID
                JOIN Route r ON q.RouteID = r.RouteID
                WHERE qe.DriverID = @driverID
                ORDER BY qe.JoinedAt DESC";

            using var conn = _dbHelper.GetConnection();
            conn.Open();

            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@driverID", driverID);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            return dt;
        }

        public DataTable GetQueueDrivers(int queueId)
        {
            string query = @"
                SELECT 
                    qe.Position AS Position,
                    u.FirstName || ' ' || u.LastName AS DriverName,
                    d.BodyNumber AS BodyNumber,
                    ds.StatusName AS Status
                FROM QueueEntry qe
                INNER JOIN Driver d ON qe.DriverID = d.DriverID
                INNER JOIN User u ON d.UserID = u.UserID
                INNER JOIN DriverStatus ds ON d.StatusID = ds.StatusID
                WHERE qe.QueueID = $queueId
                ORDER BY qe.Position ASC;
            ";

            using var conn = _dbHelper.GetConnection();
            conn.Open();

            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("$queueId", queueId);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            return dt;
        }
    }
}
