using Microsoft.Data.Sqlite;
using System;
using System.Data;
using TriQue.Helpers;
using TriQue.Models;

namespace TriQue.Data.Repositories
{
    public class QueueRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public QueueRepository()
        {
            _dbHelper = new DatabaseHelper();
        }

        #region Queue Retrieval

        public Queue? GetQueueByRouteId(int routeId)
        {
            string query = @"
                SELECT QueueID, RouteID
                FROM Queue 
                WHERE RouteID = $routeId
                LIMIT 1;
            ";

            using var conn = _dbHelper.GetConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("$routeId", routeId);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Queue
            {
                QueueID = reader.GetInt32(0),
                RouteID = reader.GetInt32(1)
            };
        }

        // status, route, and rank
        public DataRow? GetQueueDriver(int queueId, int driverId)
        {
            string query = @"
                SELECT 
                    CASE 
                        WHEN ds.StatusName = 'OnTrip' THEN '-'
                        ELSE CAST(ranked.Position AS TEXT)
                    END AS Position,
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
                    AND d.StatusID != 3        -- excludes Finished
                    AND d.StatusID != 2        -- ✅ excludes OnTrip from rank count
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

        // driver view queue status datagrid
        public DataTable GetQueueDrivers(int queueId)
        {
            string query = @"
                SELECT 
                    CASE 
                        WHEN ds.StatusName = 'OnTrip' THEN '-'
                        ELSE CAST(ROW_NUMBER() OVER (
                            PARTITION BY CASE WHEN ds.StatusName = 'OnTrip' THEN 1 ELSE 0 END
                            ORDER BY qe.Position ASC
                        ) AS TEXT)
                    END AS Position,
                    u.FirstName || ' ' || u.LastName AS DriverName,
                    d.BodyNumber AS BodyNumber,
                    ds.StatusName AS Status
                FROM QueueEntry qe
                INNER JOIN Driver d ON qe.DriverID = d.DriverID
                INNER JOIN User u ON d.UserID = u.UserID
                INNER JOIN DriverStatus ds ON d.StatusID = ds.StatusID
                WHERE qe.QueueID = $queueId
                AND d.StatusID != 3
                ORDER BY 
                    CASE WHEN ds.StatusName = 'OnTrip' THEN 1 ELSE 0 END ASC,
                    qe.Position ASC;
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

        public DataTable GetQueueByGroupID(int groupID, int routeID)
        {
            string query = @"
                SELECT 
                    CASE 
                        WHEN qe.Position IS NULL THEN '-'
                        WHEN ds.StatusName IN ('OnTrip', 'Finished') THEN '-'
                        ELSE CAST(ROW_NUMBER() OVER (
                            PARTITION BY CASE WHEN ds.StatusName = 'Waiting' AND qe.Position IS NOT NULL THEN 0 ELSE 1 END
                            ORDER BY qe.Position ASC
                        ) AS TEXT)
                    END AS Ranking,
                    d.BodyNumber,
                    u.FirstName || ' ' || u.LastName AS DriverName,
                    ds.StatusName AS TripStatus,
                    d.DriverID
                FROM Driver d
                JOIN User u ON d.UserID = u.UserID
                JOIN DriverStatus ds ON d.StatusID = ds.StatusID
                LEFT JOIN Queue q ON q.RouteID = @routeID
                LEFT JOIN QueueEntry qe ON qe.QueueID = q.QueueID 
                    AND qe.DriverID = d.DriverID
                WHERE d.GroupID = @groupID
                ORDER BY 
                    CASE WHEN ds.StatusName = 'Waiting' AND qe.Position IS NOT NULL THEN 0 ELSE 1 END ASC,
                    CASE WHEN qe.Position IS NULL THEN 1 ELSE 0 END ASC,
                    qe.Position ASC";

            using var reader = _dbHelper.ExecuteReader(query,
                new SqliteParameter("@groupID", groupID),
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

        #endregion

        #region Queue Actions

        public void AddQueueEntry(QueueEntry entry)
        {
            string query = @"
                INSERT INTO QueueEntry (QueueID, DriverID, Position, JoinedAt)
                VALUES ($queueId, $driverId, $position, $joinedAt);
            ";

            _dbHelper.ExecuteNonQuery(
                query,
                new SqliteParameter("$queueId", entry.QueueID),
                new SqliteParameter("$driverId", entry.DriverID),
                new SqliteParameter("$position", entry.QueuePosition),
                new SqliteParameter("$joinedAt", entry.JoinedAt)
            );
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


        public void ResetQueue(int routeID, int groupID)
        {
            // end open trips
            string endTrips = @"
                UPDATE Trip 
                SET EndTime = CURRENT_TIMESTAMP
                WHERE EndTime IS NULL
                AND DriverID IN (
                    SELECT DriverID FROM Driver WHERE GroupID = @groupID
                )";

            _dbHelper.ExecuteNonQuery(endTrips,
                new SqliteParameter("@groupID", groupID)
            );

            string finishOnTrip = @"
                UPDATE Driver
                SET StatusID = 3
                WHERE GroupID = @groupID
                AND StatusID = 2";

            _dbHelper.ExecuteNonQuery(finishOnTrip,
                new SqliteParameter("@groupID", groupID)
            );


            string updateStatus = @"
                UPDATE Driver
                SET StatusID = 1
                WHERE GroupID = @groupID
                AND StatusID != 3";

            _dbHelper.ExecuteNonQuery(updateStatus,
                new SqliteParameter("@groupID", groupID)
            );

            // clear queue
            string clearQueue = @"
                DELETE FROM QueueEntry
                WHERE QueueID = (
                    SELECT QueueID FROM Queue WHERE RouteID = @routeID
                )";

            _dbHelper.ExecuteNonQuery(clearQueue,
                new SqliteParameter("@routeID", routeID)
            );
        }
        #endregion

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

 
    }
}