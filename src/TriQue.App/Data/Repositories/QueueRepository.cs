using Microsoft.Data.Sqlite;
using System.Data;
using TriQue.Data.Database;

namespace TriQue.Data.Repositories
{
    public class QueueRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public QueueRepository()
        {
            _dbHelper = new DatabaseHelper();
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
    }
}
