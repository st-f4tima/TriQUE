using System;
using Microsoft.Data.Sqlite;
using TriQue.Data.Database;
using TriQue.Models;

namespace TriQue.Data.Repositories
{
    public class RouteRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public RouteRepository()
        {
            _dbHelper = new DatabaseHelper();
        }

        public Route? GetRouteByID(int routeId)
        {
            using var conn = _dbHelper.GetConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT RouteID, AssignedGroup, RouteName, StartLat, StartLng, EndLat, EndLng
                FROM Route
                WHERE RouteID = $routeId;
            ";

            cmd.Parameters.AddWithValue("$routeId", routeId);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Route
                {
                    RouteID = reader.GetInt32(0),
                    AssignedGroup = reader.GetInt32(1),
                    RouteName = reader.GetString(2),
                    StartLat = reader.IsDBNull(3) ? 0 : reader.GetDouble(3),
                    StartLng = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                    EndLat = reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                    EndLng = reader.IsDBNull(6) ? 0 : reader.GetDouble(6)
                };
            }

            return null;
        }

        public Route? GetRouteByGroupID(int groupID)
        {
            string query = @"
                SELECT RouteID, AssignedGroup, RouteName, StartLat, StartLng, EndLat, EndLng
                FROM Route
                WHERE AssignedGroup = @groupID
                LIMIT 1;
            ";

            using var conn = _dbHelper.GetConnection();
            conn.Open();

            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@groupID", groupID);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read()) return null;

            return new Route
            {
                RouteID = reader.GetInt32(0),
                AssignedGroup = reader.GetInt32(1),
                RouteName = reader.GetString(2),
                StartLat = reader.IsDBNull(3) ? 0 : reader.GetDouble(3),
                StartLng = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                EndLat = reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                EndLng = reader.IsDBNull(6) ? 0 : reader.GetDouble(6)
            };
        }
    }
}