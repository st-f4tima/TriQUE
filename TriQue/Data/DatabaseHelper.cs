using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace TriQue.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper()
        {
            string projectRoot = Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..")
            );

            string dbFolder = Path.Combine(projectRoot, "Data", "Database");

            Directory.CreateDirectory(dbFolder);

            string dbPath = Path.Combine(dbFolder, "triqueDB.db");

            _connectionString = $"Data Source={dbPath}";
        }

        public SqliteConnection GetConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        // INSERT / UPDATE / DELETE queries
        public void ExecuteNonQuery(string query, SqliteParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;

                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // returns single value
        public object ExecuteScalar(string query, SqliteParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;

                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    return cmd.ExecuteScalar();
                }
            }
        }

        // SELECT queries
        public SqliteDataReader ExecuteReader(string query, SqliteParameter[] parameters)
        {
            var conn = GetConnection();
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = query;

            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }
    }
}