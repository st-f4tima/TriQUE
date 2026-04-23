using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace TriQue.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper()
        {
            var conn = AppConfig.Configuration.GetConnectionString("Default");

            string fullPath = Path.GetFullPath(conn, AppContext.BaseDirectory);

            string? folder = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrWhiteSpace(folder))
            {
                Directory.CreateDirectory(folder);
            }

            _connectionString = $"Data Source={fullPath}";
        }

        public SqliteConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        // Insert / detete / update
        public void ExecuteNonQuery(string query, params SqliteParameter[] parameters)
        {
            using var conn = GetConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddRange(parameters);
            cmd.ExecuteNonQuery();
        }

        // Returns one
        public object? ExecuteScalar(string query, params SqliteParameter[] parameters)
        {
            using var conn = GetConnection();
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddRange(parameters);

            return cmd.ExecuteScalar();
        }

        // SELECT queries
        public SqliteDataReader ExecuteReader(string query, params SqliteParameter[] parameters)
        {
            var conn = GetConnection();
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.AddRange(parameters);

            return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }
    }
}