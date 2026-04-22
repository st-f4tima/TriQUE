using System;
using Microsoft.Data.Sqlite;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace TriQue.Data
{
    internal class DatabaseInitializer
    {
        private readonly string _connectionString;

        public  DatabaseInitializer()
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
        public void Initialize()
        {
            using (var conn = new SqliteConnection(_connectionString))
            {
                conn.Open();

                var cmd = conn.CreateCommand();

                // not final - just for the first feature
                cmd.CommandText =
                @"
                CREATE TABLE IF NOT EXISTS UserRole (
                    RoleID INTEGER PRIMARY KEY,
                    RoleName TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS AdminLevel (
                    LevelID INTEGER PRIMARY KEY,
                    LevelName TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS DriverStatus (
                    StatusID INTEGER PRIMARY KEY,
                    StatusName TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS User (
                    UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT NOT NULL,
                    PasswordHash TEXT NOT NULL,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    PhoneNumber TEXT NOT NULL,
                    RoleID INTEGER NOT NULL,
                    FailedAttempts INTEGER NOT NULL DEFAULT 0,
                    LockoutUntil DATETIME,
                    FOREIGN KEY (RoleID) REFERENCES UserRole(RoleID)
                );

                CREATE TABLE IF NOT EXISTS AuthenticationLog (
                    LogID INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserID INTEGER NOT NULL,
                    LoginTime DATETIME,
                    LogoutTime DATETIME,
                    LogoutOutcome TEXT NOT NULL,
                    FOREIGN KEY (UserID) REFERENCES User(UserID)
                );
                ";

                cmd.ExecuteNonQuery();

                cmd.CommandText =
                @"
                INSERT OR IGNORE INTO UserRole (RoleID, RoleName) VALUES
                    (1, 'Driver'),
                    (2, 'Admin');

                INSERT OR IGNORE INTO AdminLevel (LevelID, LevelName) VALUES
                    (1, 'SuperAdmin'),
                    (2, 'TodaOfficer'),
                    (3, 'Staff');

                INSERT OR IGNORE INTO DriverStatus (StatusID, StatusName) VALUES
                    (1, 'Waiting'),
                    (2, 'OnTrip'),
                    (3, 'Finished');

                INSERT OR IGNORE INTO User 
                (Username, PasswordHash, FirstName, LastName, PhoneNumber, RoleID, FailedAttempts, LockoutUntil)
                VALUES
                    ('admin1', 'hashed_admin_pass', 'Juan', 'Dela Cruz', '09123456789', 2, 0, NULL),
                    ('driver1', 'hashed_driver_pass', 'Pedro', 'Santos', '09987654321', 1, 0, NULL),
                    ('driver2', 'hashed_driver_pass', 'Maria', 'Reyes', '09111222333', 1, 0, NULL);
                ";

                cmd.ExecuteNonQuery();
            }
        }
    }
}