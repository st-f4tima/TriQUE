using Microsoft.Data.Sqlite;
using System;
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
                new Microsoft.Data.Sqlite.SqliteParameter("$routeId", routeId)
            );

            if (result == null)
                throw new Exception("Queue not found for this route.");

            return Convert.ToInt32(result);
        }

        // driver view queue status (top)
        public DataRow? GetQueueDriver(int queueId, int driverId)
        {
            string query = @"
                SELECT 
                    ranked.Position,
                    r.RouteName,
                    ds.StatusName AS Status
                FROM (
                SELECT 
                    qe.DriverID,
                    ROW_NUMBER() OVER (ORDER BY qe.Position ASC) AS Position,
                    q.RouteID
                FROM QueueEntry qe
                INNER JOIN Queue q ON qe.QueueID = q.QueueID
                INNER JOIN Driver d ON qe.DriverID = d.DriverID
                WHERE qe.QueueID = $queueId
                AND d.StatusID != 3
                ) ranked
                INNER JOIN Route r ON ranked.RouteID = r.RouteID
                INNER JOIN Driver d ON ranked.DriverID = d.DriverID
                INNER JOIN DriverStatus ds ON d.StatusID = ds.StatusID
                WHERE ranked.DriverID = $driverId
                LIMIT 1;
            ";

            using var conn = _dbHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("$queueId", queueId);
            cmd.Parameters.AddWithValue("$driverId", driverId);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public void AddQueueEntry(int queueId, int driverId, int position)
        {
            string query = @"
                INSERT INTO QueueEntry (QueueID, DriverID, Position, JoinedAt)
                VALUES ($queueId, $driverId, $position, datetime('now'));
            ";

            _dbHelper.ExecuteNonQuery(
                query,
                new Microsoft.Data.Sqlite.SqliteParameter("$queueId", queueId),
                new Microsoft.Data.Sqlite.SqliteParameter("$driverId", driverId),
                new Microsoft.Data.Sqlite.SqliteParameter("$position", position)
            );
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
                new Microsoft.Data.Sqlite.SqliteParameter("$queueId", queueId)
            );

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
                new Microsoft.Data.Sqlite.SqliteParameter("$queueId", queueId),
                new Microsoft.Data.Sqlite.SqliteParameter("$driverId", driverId)
            );

            return Convert.ToInt32(result) > 0;
        }

        public void ReorderQueuePositions(int queueId)
        {
            string createTemp = @"
                CREATE TEMP TABLE IF NOT EXISTS _reorder AS
                SELECT DriverID, ROW_NUMBER() OVER (ORDER BY Position ASC) AS NewPos
                FROM QueueEntry
                WHERE QueueID = $queueId;
            ";

            string query = @"
                UPDATE QueueEntry
                SET Position = (
                    SELECT COUNT(*)
                    FROM QueueEntry q2
                    WHERE q2.QueueID = QueueEntry.QueueID
                    AND q2.Position <= QueueEntry.Position
                )
                WHERE QueueID = $queueId;
            ";

            _dbHelper.ExecuteNonQuery(query,
                new SqliteParameter("$queueId", queueId));
        }

        public void RemoveDriverFromQueue(int driverID, int queueID)
        {
            string query = @"
                DELETE FROM QueueEntry
                WHERE DriverID = $driverID AND QueueID = $queueID;
            ";
            _dbHelper.ExecuteNonQuery(query,
                new SqliteParameter("$driverID", driverID),
                new SqliteParameter("$queueID", queueID));
        }

        // driver dashboard datagrid
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
                WHERE qe.DriverID = $driverID
                ORDER BY qe.JoinedAt DESC;
                 ";

            using var conn = _dbHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("$driverID", driverID);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            return dt;
        }

        // driver view queue status datagrid
        public DataTable GetQueueDrivers(int queueId)
        {
            string query = @"
                SELECT 
                    ROW_NUMBER() OVER (ORDER BY qe.Position ASC) AS Position,
                    u.FirstName || ' ' || u.LastName AS DriverName,
                    d.BodyNumber AS BodyNumber,
                    ds.StatusName AS Status
                FROM QueueEntry qe
                INNER JOIN Driver d ON qe.DriverID = d.DriverID
                INNER JOIN User u ON d.UserID = u.UserID
                INNER JOIN DriverStatus ds ON d.StatusID = ds.StatusID
                WHERE qe.QueueID = $queueId
                AND d.StatusID != 3
                ORDER BY qe.Position ASC;
            ";

            using var conn = _dbHelper.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("$queueId", queueId);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            return dt;
        }
    }
}