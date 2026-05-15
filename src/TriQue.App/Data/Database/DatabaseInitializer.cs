using System;
using Microsoft.Data.Sqlite;
using System.IO;
using Microsoft.Extensions.Configuration;
using TriQue.Helpers;

namespace TriQue.Data.Database
{
    public class DatabaseInitializer
    {
        private readonly DatabaseHelper _dbHelper;
        private readonly IConfiguration _config;


        public DatabaseInitializer(DatabaseHelper db, IConfiguration config)
        {
            _dbHelper = db;
            _config = config;
        }

        #region Database Initialization
        public void Initialize()
        {
            using var conn = _dbHelper.GetConnection();
            conn.Open();

            var cmd = conn.CreateCommand();

            // Uncomment this if AuthenticationServiceTests.cs keeps failing
            //cmd.CommandText =
            //@"
            //    PRAGMA foreign_keys = OFF;
            //    DROP TABLE IF EXISTS AuthenticationLog;
            //    DROP TABLE IF EXISTS QueueEntry;
            //    DROP TABLE IF EXISTS Trip;
            //    DROP TABLE IF EXISTS Queue;
            //    DROP TABLE IF EXISTS Driver;
            //    DROP TABLE IF EXISTS Admin;
            //    DROP TABLE IF EXISTS User;
            //    DROP TABLE IF EXISTS Route;
            //    DROP TABLE IF EXISTS DriverGroup;
            //    DROP TABLE IF EXISTS DriverStatus;
            //    DROP TABLE IF EXISTS AdminLevel;
            //    DROP TABLE IF EXISTS UserRole;
            //    DROP TABLE IF EXISTS TrafficLog;
            //    PRAGMA foreign_keys = ON;
            //";
            //cmd.ExecuteNonQuery();

            // creating tables
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

            CREATE TABLE IF NOT EXISTS DriverGroup (
                GroupID INTEGER PRIMARY KEY AUTOINCREMENT,
                GroupName TEXT NOT NULL,
                RotationDay INTEGER NOT NULL
            );

            CREATE TABLE IF NOT EXISTS Route (
                RouteID INTEGER PRIMARY KEY AUTOINCREMENT,
                AssignedGroup INTEGER NOT NULL,
                RouteName TEXT NOT NULL,
                StartLat REAL,
                StartLng REAL,
                EndLat REAL,
                EndLng REAL,
                DistanceKm REAL,
                FOREIGN KEY (AssignedGroup) REFERENCES DriverGroup(GroupID)
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
                IsTemporaryPassword INTEGER NOT NULL DEFAULT 0,
                FOREIGN KEY (RoleID) REFERENCES UserRole(RoleID)
            );

            CREATE TABLE IF NOT EXISTS AuthenticationLog (
                LogID INTEGER PRIMARY KEY AUTOINCREMENT,
                UserID INTEGER NOT NULL,
                LoginTime DATETIME NOT NULL,
                LogoutTime DATETIME,
                AuthOutcome TEXT NOT NULL,
                FOREIGN KEY (UserID) REFERENCES User(UserID)
            );


            CREATE TABLE IF NOT EXISTS Driver (
                DriverID INTEGER PRIMARY KEY AUTOINCREMENT,
                UserID INTEGER NOT NULL,
                GroupID INTEGER NOT NULL,
                StatusID INTEGER NOT NULL,
                BodyNumber TEXT NOT NULL,
                GoalEarnings REAL DEFAULT 650,
                FOREIGN KEY (UserID) REFERENCES User(UserID),
                FOREIGN KEY (GroupID) REFERENCES DriverGroup(GroupID),
                FOREIGN KEY (StatusID) REFERENCES DriverStatus(StatusID)
            );

            CREATE TABLE IF NOT EXISTS Admin (
                AdminID INTEGER PRIMARY KEY AUTOINCREMENT,
                UserID INTEGER NOT NULL,
                LevelID INTEGER NOT NULL,
                FOREIGN KEY (UserID) REFERENCES User(UserID),
                FOREIGN KEY (LevelID) REFERENCES AdminLevel(LevelID)
            );

            CREATE TABLE IF NOT EXISTS Queue (
                QueueID INTEGER PRIMARY KEY AUTOINCREMENT,
                RouteID INTEGER NOT NULL,
                FOREIGN KEY (RouteID) REFERENCES Route(RouteID)
            );

            CREATE TABLE IF NOT EXISTS Trip (
                TripID INTEGER PRIMARY KEY AUTOINCREMENT,
                DriverID INTEGER NOT NULL,
                RouteID INTEGER NOT NULL,
                StatusID INTEGER NOT NULL,
                ActualEarnings REAL NOT NULL,
                StartTime DATETIME NOT NULL,
                EndTime DATETIME,
                FOREIGN KEY (RouteID) REFERENCES Route(RouteID),
                FOREIGN KEY (DriverID) REFERENCES Driver(DriverID),
                FOREIGN KEY (StatusID) REFERENCES DriverStatus(StatusID)
            );

            CREATE TABLE IF NOT EXISTS QueueEntry (
                EntryID INTEGER PRIMARY KEY AUTOINCREMENT,
                QueueID INTEGER NOT NULL,
                DriverID INTEGER NOT NULL,
                Position INTEGER NOT NULL,
                JoinedAt DATETIME,
                FOREIGN KEY (DriverID) REFERENCES Driver(DriverID),
                FOREIGN KEY (QueueID) REFERENCES Queue(QueueID)
            );

            CREATE TABLE IF NOT EXISTS TrafficLog (
                    LogID INTEGER PRIMARY KEY AUTOINCREMENT,
                    RouteID INTEGER NOT NULL,
                    DelaySec REAL NOT NULL,
                    TrafficLevel TEXT NOT NULL,
                    FetchedAt DATETIME NOT NULL DEFAULT (datetime('now')),
                    FOREIGN KEY (RouteID) REFERENCES Route(RouteID)
                );
            ";

            cmd.ExecuteNonQuery();

            // inserting data
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

            INSERT OR IGNORE INTO DriverGroup (GroupID, GroupName, RotationDay) VALUES
                (1, 'Group A', 1),
                (2, 'Group B', 2),
                (3, 'Group C', 3),
                (4, 'Group D', 4),
                (5, 'Group E', 5),
                (6, 'Group F', 6);


            INSERT OR IGNORE INTO Route (RouteID, AssignedGroup, RouteName, StartLat, StartLng, EndLat, EndLng, DistanceKm) VALUES
                (101, 1, 'Provincial Capitol',  13.79277, 121.07137, 13.76527, 121.06423, 4.8),
                (102, 2, 'Grand Terminal',      13.79277, 121.07137, 13.79058, 121.06161, 2.4),
                (103, 3, 'SM Batangas',         13.79277, 121.07137, 13.75546, 121.06842, 7.5),
                (104, 4, 'WalterMart',          13.79277, 121.07137, 13.76397, 121.05640, 5.3),
                (105, 5, 'Brgy. Tulo',          13.79277, 121.07137, 13.75460, 121.12638, 11),
                (106, 6, 'BSU Alangilan',       13.79277, 121.07137, 13.78414, 121.07439, 2.8);

            INSERT OR IGNORE INTO Queue (QueueID, RouteID) VALUES
                (1, 101),
                (2, 102),
                (3, 103),
                (4, 104),
                (5, 105),
                (6, 106);
            ";
            cmd.ExecuteNonQuery();

            SeedUsers();
        }

        #endregion

        #region Seed Users
        // seeds user table
        private void SeedUsers()
        {
            string adminDefault = _config["SeedPasswords:AdminDefault"];

            if (string.IsNullOrEmpty(adminDefault))
            {
                throw new Exception("AdminDefault seed password not set.");
            }

            string driverDefault = _config["SeedPasswords:DriverDefault"];

            if (string.IsNullOrEmpty(driverDefault))
            {
                throw new Exception("DriverDefault seed password not set.");
            }

            int userID = 1;
            int adminID = 1;

            // 3 SuperAdmins
            for (int i = 1; i <= 3; i++, userID++, adminID++)
            {
                InsertUserIfNotExists(
                    userID, 
                    $"admin{userID}", 
                    adminDefault, 
                    $"SuperAdmin{i}", 
                    "Test", 
                    $"0911000{userID:D4}", 
                    roleID: 2, 
                    isTempPassword: false
                );
                InsertAdminIfNotExists(adminID, userID, levelID: 1);
            }

            // 7 TodaOfficers
            for (int i = 1; i <= 7; i++, userID++, adminID++)
            {
                InsertUserIfNotExists(
                    userID, 
                    $"admin{userID}", 
                    adminDefault, 
                    $"TodaOfficer{i}", 
                    "Test", 
                    $"0912000{userID:D4}", 
                    roleID: 2, 
                    isTempPassword: false
                );
                InsertAdminIfNotExists(adminID, userID, levelID: 2);
            }

            // 10 Staff
            for (int i = 1; i <= 10; i++, userID++, adminID++)
            {
                InsertUserIfNotExists(userID, 
                    $"admin{userID}", 
                    adminDefault, 
                    $"Staff{i}", 
                    "Test", 
                    $"0913000{userID:D4}", 
                    roleID: 2, 
                    isTempPassword: 
                    false
                );
                InsertAdminIfNotExists(adminID, userID, levelID: 3);
            }

            // 120 Drivers 
            for (int i = 1; i <= 120; i++, userID++)
            {
                int groupID = ((i - 1) / 20) + 1;

                InsertUserIfNotExists(
                    userID, 
                    $"driver{i}", 
                    driverDefault, 
                    $"Driver{i}", 
                    "Test", 
                    $"0930000{i:D4}", 
                    roleID: 1, 
                    isTempPassword: 
                    false
                );
                InsertDriverIfNotExists(i, userID, groupID, statusID: 1, $"TN-{i:D3}");

                // Test Account (Admin)
                InsertUserIfNotExists(
                    999,
                    "testuser",
                    "Test123!",
                    "Test",
                    "Account",
                    "09999999999",
                    roleID: 2,
                    isTempPassword: false
                );

                InsertAdminIfNotExists(
                    adminID: 999,
                    userID: 999,
                    levelID: 1
                );

                // Test Account (Driver)
                InsertUserIfNotExists(
                    1000,
                    "testdriver",
                    "Driver123!",
                    "Test",
                    "Driver",
                    "09888888888",
                    roleID: 1,
                    isTempPassword: false
                );

                InsertDriverIfNotExists(
                    driverID: 1000,
                    userID: 1000,
                    groupID: 1,
                    statusID: 1,
                    bodyNum: "TN-999"
                );
            }
        }

        #endregion

        #region Insert Methods
        private void InsertUserIfNotExists(int id, string username, string password, 
            string first, string last, string phone, int roleID, bool isTempPassword)
        {
            var exists = (long)_dbHelper.ExecuteScalar("SELECT COUNT(*) FROM User WHERE UserID = @id",
                new SqliteParameter("@id", id)) > 0;

            if (exists) return;

            _dbHelper.ExecuteNonQuery(@"
                INSERT INTO User (UserID, Username, PasswordHash, FirstName, LastName, PhoneNumber, RoleID, IsTemporaryPassword)
                VALUES (@id, @u, @p, @f, @l, @ph, @r, @tmp)",
                new SqliteParameter("@id", id),
                new SqliteParameter("@u", username),
                new SqliteParameter("@p", PasswordHelper.Hash(password)),
                new SqliteParameter("@f", first),
                new SqliteParameter("@l", last),
                new SqliteParameter("@ph", phone),
                new SqliteParameter("@r", roleID),
                new SqliteParameter("@tmp", isTempPassword ? 1 : 0));
        }

        private void InsertDriverIfNotExists(int driverID, int userID, int groupID, int statusID, string bodyNum)
        {
            var exists = (long)_dbHelper.ExecuteScalar("SELECT COUNT(*) FROM Driver WHERE DriverID = @id",
                new SqliteParameter("@id", driverID)) > 0;

            if (exists) return;

            _dbHelper.ExecuteNonQuery(@"
                INSERT INTO Driver (DriverID, UserID, GroupID, StatusID, BodyNumber)
                VALUES (@did, @uid, @gid, @sid, @bn)",
                new SqliteParameter("@did", driverID),
                new SqliteParameter("@uid", userID),
                new SqliteParameter("@gid", groupID),
                new SqliteParameter("@sid", statusID),
                new SqliteParameter("@bn", bodyNum));
        }

        private void InsertAdminIfNotExists(int adminID, int userID, int levelID)
        {
            var exists = (long)_dbHelper.ExecuteScalar("SELECT COUNT(*) FROM Admin WHERE AdminID = @id",
                new SqliteParameter("@id", adminID)) > 0;
            if (exists) return;

            _dbHelper.ExecuteNonQuery(@"
                INSERT INTO Admin (AdminID, UserID, LevelID) VALUES (@aid, @uid, @lid)",
                new SqliteParameter("@aid", adminID),
                new SqliteParameter("@uid", userID),
                new SqliteParameter("@lid", levelID));
        }

        #endregion
    }
}