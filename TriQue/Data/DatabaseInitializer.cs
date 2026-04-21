using System;
using Microsoft.Data.Sqlite;
using System.IO;

namespace TriQue.Data
{
    internal class DatabaseInitializer
    {
        public void Initialize()
        {
            string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
            string dbFolder = Path.Combine(projectRoot, "Data", "Database");

            Directory.CreateDirectory(dbFolder);

            string dbPath = Path.Combine(dbFolder, "triqueDB.db");
            string connectionString = $"Data Source={dbPath}";

            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                var cmd = conn.CreateCommand();

                // not final - just for the first feature
                cmd.CommandText =
                @"
                CREATE TABLE IF NOT EXISTS UserRole (
                    roleID INTEGER PRIMARY KEY,
                    roleName TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS AdminLevel (
                    levelID INTEGER PRIMARY KEY,
                    levelName TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS DriverStatus (
                    statusID INTEGER PRIMARY KEY,
                    statusName TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS User (
                    userID INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT NOT NULL,
                    passwordHash TEXT NOT NULL,
                    firstName TEXT NOT NULL,
                    lastName TEXT NOT NULL,
                    phoneNumber TEXT NOT NULL,
                    roleID INTEGER NOT NULL,
                    failedAttempts INTEGER NOT NULL DEFAULT 0,
                    lockoutUntil DATETIME,
                    FOREIGN KEY (roleID) REFERENCES UserRole(roleID)
                );

                CREATE TABLE IF NOT EXISTS AuthenticationLog (
                    logID INTEGER PRIMARY KEY AUTOINCREMENT,
                    userID INTEGER NOT NULL,
                    loginTime DATETIME,
                    logoutTime DATETIME,
                    logoutOutcome TEXT NOT NULL,
                    FOREIGN KEY (userID) REFERENCES User(userID)
                );
                ";

                cmd.ExecuteNonQuery();

                cmd.CommandText =
                @"
                INSERT OR IGNORE INTO UserRole (roleID, roleName) VALUES
                    (1, 'Driver'),
                    (2, 'Admin');

                INSERT OR IGNORE INTO AdminLevel (levelID, levelName) VALUES
                    (1, 'SuperAdmin'),
                    (2, 'TodaOfficer'),
                    (3, 'Staff');

                INSERT OR IGNORE INTO DriverStatus (statusID, statusName) VALUES
                    (1, 'Waiting'),
                    (2, 'OnTrip'),
                    (3, 'Finished');

                INSERT OR IGNORE INTO User 
                (username, passwordHash, firstName, lastName, phoneNumber, roleID, failedAttempts, lockoutUntil)
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