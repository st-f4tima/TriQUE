using Microsoft.Data.Sqlite;
using TriQue.Helpers;

namespace TriQue.Data.Repositories
{
    public class TrafficRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public TrafficRepository()
        {
            _dbHelper = new DatabaseHelper();
        }
        public void SaveTrafficLog(int routeID, double delaySec, string level)
        {
            string query = @"
                INSERT INTO TrafficLog (RouteID, DelaySec, TrafficLevel)
                VALUES (@routeId, @delay, @level)
            ";

            _dbHelper.ExecuteNonQuery(query,
                new SqliteParameter("@routeId", routeID),
                new SqliteParameter("@delay", delaySec),
                new SqliteParameter("@level", level)
            );
        }
        public bool IsTrafficProne(int routeId)
        {
            string query = @"
                SELECT COUNT(*) FROM TrafficLog
                WHERE RouteID = @routeId
                  AND TrafficLevel IN ('Heavy', 'Moderate')
                  AND FetchedAt >= datetime('now', '-7 days')
            ";

            var result = _dbHelper.ExecuteScalar(query,
                new SqliteParameter("@routeId", routeId));

            return result != null && (long)result >= 5;
        }

        public string GetPeakWindow(int routeId)
        {
            string query = @"
                SELECT strftime('%H', FetchedAt) as Hour, COUNT(*) as Hits
                FROM TrafficLog
                WHERE RouteID = @routeId
                  AND TrafficLevel IN ('Heavy', 'Moderate')
                GROUP BY Hour
                ORDER BY Hits DESC
                LIMIT 1
            ";

            using var conn = _dbHelper.GetConnection();
            conn.Open();

            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@routeId", routeId);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read()) return "No Data Yet";

            int peakHour = int.Parse(reader.GetString(0));
            int endHour = peakHour + 2;

            // format to 12-hour
            string FormatHour(int h)
            {
                string suffix = h < 12 ? "AM" : "PM";
                int h12 = h % 12 == 0 ? 12 : h % 12;
                return $"{h12}:00 {suffix}";
            }

            return $"{FormatHour(peakHour)} – {FormatHour(endHour)}";
        }
    }
}