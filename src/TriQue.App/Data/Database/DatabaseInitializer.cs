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

        public DatabaseInitializer(DatabaseHelper db)
        {
            _dbHelper = db;
        }
        public void Initialize()
        {
            using var conn = _dbHelper.GetConnection();
            conn.Open();

            var cmd = conn.CreateCommand();

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


            INSERT OR IGNORE INTO User (UserID, Username, PasswordHash, FirstName, LastName, PhoneNumber, RoleID, IsTemporaryPassword) VALUES
                -- ADMINS
                (1, 'admin1', 'hash1', 'Juan', 'Dela Cruz', '09111111111', 2, 0),
                (2, 'admin2', 'hash2', 'Luis', 'Torres', '09222222222', 2, 0),

                -- DRIVERS (30)
                (3, 'driver1', 'hash3', 'Mike', 'Wheeler', '09300000001', 1, 1),
                (4, 'driver2', 'hash4', 'Driver2', 'Test', '09300000002', 1, 0),
                (5, 'driver3', 'hash5', 'Driver3', 'Test', '09300000003', 1, 0),
                (6, 'driver4', 'hash6', 'Driver4', 'Test', '09300000004', 1, 0),
                (7, 'driver5', 'hash7', 'Driver5', 'Test', '09300000005', 1, 0),
                (8, 'driver6', 'hash8', 'Driver6', 'Test', '09300000006', 1, 0),
                (9, 'driver7', 'hash9', 'Driver7', 'Test', '09300000007', 1, 0),
                (10, 'driver8', 'hash10', 'Driver8', 'Test', '09300000008', 1, 0),
                (11, 'driver9', 'hash11', 'Driver9', 'Test', '09300000009', 1, 0),
                (12, 'driver10', 'hash12', 'Driver10', 'Test', '09300000010', 1, 0),
                (13, 'driver11', 'hash13', 'Driver11', 'Test', '09300000011', 1, 0),
                (14, 'driver12', 'hash14', 'Driver12', 'Test', '09300000012', 1, 0),
                (15, 'driver13', 'hash15', 'Driver13', 'Test', '09300000013', 1, 0),
                (16, 'driver14', 'hash16', 'Driver14', 'Test', '09300000014', 1, 0),
                (17, 'driver15', 'hash17', 'Driver15', 'Test', '09300000015', 1, 0),
                (18, 'driver16', 'hash18', 'Driver16', 'Test', '09300000016', 1, 0),
                (19, 'driver17', 'hash19', 'Driver17', 'Test', '09300000017', 1, 0),
                (20, 'driver18', 'hash20', 'Driver18', 'Test', '09300000018', 1, 0),
                (21, 'driver19', 'hash21', 'Driver19', 'Test', '09300000019', 1, 0),
                (22, 'driver20', 'hash22', 'Driver20', 'Test', '09300000020', 1, 0),
                (23, 'driver21', 'hash23', 'Driver21', 'Test', '09300000021', 1, 0),
                (24, 'driver22', 'hash24', 'Driver22', 'Test', '09300000022', 1, 0),
                (25, 'driver23', 'hash25', 'Driver23', 'Test', '09300000023', 1, 0),
                (26, 'driver24', 'hash26', 'Driver24', 'Test', '09300000024', 1, 0),
                (27, 'driver25', 'hash27', 'Driver25', 'Test', '09300000025', 1, 0),
                (28, 'driver26', 'hash28', 'Driver26', 'Test', '09300000026', 1, 0),
                (29, 'driver27', 'hash29', 'Driver27', 'Test', '09300000027', 1, 0),
                (30, 'driver28', 'hash30', 'Driver28', 'Test', '09300000028', 1, 0),
                (31, 'driver29', 'hash31', 'Driver29', 'Test', '09300000029', 1, 0),
                (32, 'driver30', 'hash32', 'Driver30', 'Test', '09300000030', 1, 0);

            INSERT OR IGNORE INTO Admin (AdminID, UserID, LevelID) VALUES
                (1, 1, 1),
                (2, 2, 2);


            INSERT OR IGNORE INTO DriverGroup (GroupID, GroupName, RotationDay) VALUES
                (1, 'Group A', 1),
                (2, 'Group B', 2),
                (3, 'Group C', 3),
                (4, 'Group D', 4),
                (5, 'Group E', 5),
                (6, 'Group F', 6);


            INSERT OR IGNORE INTO Driver (DriverID, UserID, GroupID, StatusID, BodyNumber) VALUES

                -- A
                (1, 3, 1, 1, 'TN-001'),
                (2, 4, 1, 1, 'TN-002'),
                (3, 5, 1, 1, 'TN-003'),
                (4, 6, 1, 1, 'TN-004'),
                (5, 7, 1, 1, 'TN-005'),

                -- B
                (6, 8, 2, 1, 'TN-006'),
                (7, 9, 2, 1, 'TN-007'),
                (8, 10, 2, 1, 'TN-008'),
                (9, 11, 2, 1, 'TN-009'),
                (10, 12, 2, 1, 'TN-010'),

                -- C
                (11, 13, 3, 1, 'TN-011'),
                (12, 14, 3, 1, 'TN-012'),
                (13, 15, 3, 1, 'TN-013'),
                (14, 16, 3, 1, 'TN-014'),
                (15, 17, 3, 1, 'TN-015'),

                -- D
                (16, 18, 4, 1, 'TN-016'),
                (17, 19, 4, 1, 'TN-017'),
                (18, 20, 4, 1, 'TN-018'),
                (19, 21, 4, 1, 'TN-019'),
                (20, 22, 4, 1, 'TN-020'),

                -- E
                (21, 23, 5, 1, 'TN-021'),
                (22, 24, 5, 1, 'TN-022'),
                (23, 25, 5, 1, 'TN-023'),
                (24, 26, 5, 1, 'TN-024'),
                (25, 27, 5, 1, 'TN-025'),

                -- F
                (26, 28, 6, 1, 'TN-026'),
                (27, 29, 6, 1, 'TN-027'),
                (28, 30, 6, 1, 'TN-028'),
                (29, 31, 6, 1, 'TN-029'),
                (30, 32, 6, 1, 'TN-030');

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

            INSERT OR IGNORE INTO Trip (TripID, DriverID, RouteID, StatusID, ActualEarnings, StartTime, EndTime) VALUES
                (1, 1, 106, 3, 120, '2026-04-29 08:00:00', '2026-04-29 08:30:00'),
                (5, 1, 106, 3, 180, '2026-04-23 10:00:00', '2026-04-23 10:40:00'),
                (6, 1, 106, 3, 220, '2026-04-23 12:00:00', '2026-04-23 12:50:00'),
                (7, 1, 106, 3, 0, '2026-04-23 14:00:00', NULL),

                (2, 6, 102, 2, 150, '2026-04-23 08:10:00', '2026-04-23 08:45:00'),
                (3, 11, 103, 1, 0, '2026-04-23 09:00:00', NULL),
                (4, 16, 104, 3, 200, '2026-04-23 07:00:00', '2026-04-23 07:40:00');

            INSERT OR IGNORE INTO AuthenticationLog (LogID, UserID, LoginTime, LogoutTime, AuthOutcome) VALUES
                (1, 1, '2026-04-23 07:00:00', '2026-04-23 12:00:00', 'Success'),
                (2, 3, '2026-04-23 07:10:00', '2026-04-23 08:00:00', 'Success'),
                (3, 4, '2026-04-23 08:00:00', '2026-04-23 09:00:00', 'Success'),
                (4, 5, '2026-04-23 08:30:00', '2026-04-23 09:30:00', 'Failed');
            ";

            HashPlainPasswords();
            cmd.ExecuteNonQuery();
        }

        // Hash the initialized password 
        private void HashPlainPasswords()
        {
            string selectQuery = "SELECT UserID, PasswordHash FROM User";
            using var reader = _dbHelper.ExecuteReader(selectQuery);

            var toUpdate = new List<(int id, string hash)>();

            while (reader.Read())
            {
                string stored = reader["PasswordHash"].ToString() ?? "";

                if (!stored.StartsWith("$2"))
                {
                    string hashed = PasswordHelper.Hash(stored);
                    toUpdate.Add((Convert.ToInt32(reader["UserID"]), hashed));
                }
            }

            foreach (var (id, hash) in toUpdate)
            {
                _dbHelper.ExecuteNonQuery(
                    "UPDATE User SET PasswordHash = @hash WHERE UserID = @id",
                    new SqliteParameter("@hash", hash),
                    new SqliteParameter("@id", id));
            }
        }
    }
}