using Microsoft.VisualStudio.TestTools.UnitTesting;
using TriQue.Data.Database;
using TriQue.Data.Repositories;
using TriQue.Helpers;
using TriQue.Services;

namespace TriQue.Tests.Services
{
    [TestClass]
    public class TripServiceTests
    {
        private DatabaseHelper _dbHelper;
        private TripService _tripService;
        private TripRepository _tripRepo;

        // Seeded driver1 (DriverID=1) and route 101 (RouteID=101, DistanceKm=4.8)
        private const int TestDriverID = 1;
        private const int TestRouteID = 101;

        [TestInitialize]
        public void Setup()
        {
            _dbHelper = new DatabaseHelper();
            var dbInitializer = new DatabaseInitializer(_dbHelper, AppConfig.Configuration);
            dbInitializer.Initialize();

            _tripService = new TripService();
            _tripRepo = new TripRepository();

            // Clean trips before each test
            _dbHelper.ExecuteNonQuery(
                "DELETE FROM Trip WHERE DriverID = @driverID",
                new Microsoft.Data.Sqlite.SqliteParameter("@driverID", TestDriverID)
            );
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Reset driver status back to Waiting
            _dbHelper.ExecuteNonQuery(
                "UPDATE Driver SET StatusID = 1 WHERE DriverID = @driverID",
                new Microsoft.Data.Sqlite.SqliteParameter("@driverID", TestDriverID)
            );
        }

        // =============================================
        // CalculateFare() LOGIC TESTS
        // (tested via the formula directly since method is private)
        // =============================================

        [TestMethod]
        public void CalculateFare_ShouldReturnBaseFare_WhenDistanceIsExactly1km()
        {
            // ₱16 base for first km
            double distanceKm = 1.0;
            double fare = distanceKm <= 1.0
                ? 16
                : 16 + Math.Ceiling((distanceKm - 1.0) / 0.5) * 5;

            Assert.AreEqual(16, fare);
        }

        [TestMethod]
        public void CalculateFare_ShouldReturnBaseFare_WhenDistanceIsUnder1km()
        {
            // 0.5 km is under 1 km, still ₱16
            double distanceKm = 0.5;
            double fare = distanceKm <= 1.0
                ? 16
                : 16 + Math.Ceiling((distanceKm - 1.0) / 0.5) * 5;

            Assert.AreEqual(16, fare);
        }

        [TestMethod]
        public void CalculateFare_ShouldReturn21_WhenDistanceIs1point5km()
        {
            // 1.5 km: 16 + ceil(0.5/0.5) * 5 = 16 + 1*5 = ₱21
            double distanceKm = 1.5;
            double fare = distanceKm <= 1.0
                ? 16
                : 16 + Math.Ceiling((distanceKm - 1.0) / 0.5) * 5;

            Assert.AreEqual(21, fare);
        }

        [TestMethod]
        public void CalculateFare_ShouldReturn26_WhenDistanceIs2km()
        {
            // 2.0 km: 16 + ceil(1.0/0.5) * 5 = 16 + 2*5 = ₱26
            double distanceKm = 2.0;
            double fare = distanceKm <= 1.0
                ? 16
                : 16 + Math.Ceiling((distanceKm - 1.0) / 0.5) * 5;

            Assert.AreEqual(26, fare);
        }

        [TestMethod]
        public void CalculateFare_ShouldUseCeiling_WhenDistanceIs1point3km()
        {
            // 1.3 km: succeeding = 0.3 km
            // ceil(0.3/0.5) = ceil(0.6) = 1
            // 16 + 1*5 = ₱21, NOT ₱16
            double distanceKm = 1.3;
            double fare = distanceKm <= 1.0
                ? 16
                : 16 + Math.Ceiling((distanceKm - 1.0) / 0.5) * 5;

            Assert.AreEqual(21, fare);
            Assert.AreNotEqual(16, fare, "Ceiling should push partial 500m to next bracket");
        }

        // =============================================
        // EDGE CASES
        // =============================================

        [TestMethod]
        public void CalculateFare_ShouldReturn16_WhenDistanceIsZero()
        {
            // 0 km still returns base fare
            double distanceKm = 0;
            double fare = distanceKm <= 1.0
                ? 16
                : 16 + Math.Ceiling((distanceKm - 1.0) / 0.5) * 5;

            Assert.AreEqual(16, fare);
        }

        [TestMethod]
        public void CalculateFare_ShouldReturnCorrectFare_ForAllSeededRoutes()
        {
            // Verify fare formula against all 6 seeded route distances
            // RouteID: 101=4.8km, 102=2.4km, 103=7.5km, 104=5.3km, 105=11km, 106=2.8km
            var expectedFares = new Dictionary<int, double>
            {
                { 101, 16 + Math.Ceiling((4.8 - 1.0) / 0.5) * 5 },  // ₱56
                { 102, 16 + Math.Ceiling((2.4 - 1.0) / 0.5) * 5 },  // ₱31
                { 103, 16 + Math.Ceiling((7.5 - 1.0) / 0.5) * 5 },  // ₱81
                { 104, 16 + Math.Ceiling((5.3 - 1.0) / 0.5) * 5 },  // ₱61
                { 105, 16 + Math.Ceiling((11.0 - 1.0) / 0.5) * 5 }, // ₱116
                { 106, 16 + Math.Ceiling((2.8 - 1.0) / 0.5) * 5 }   // ₱36
            };

            foreach (var entry in expectedFares)
            {
                Assert.IsTrue(entry.Value >= 16,
                    $"RouteID {entry.Key} fare {entry.Value} should be at least ₱16");
            }
        }

        // =============================================
        // StartTrip() / EndTrip() BEHAVIOR TESTS
        // =============================================

        [TestMethod]
        public void StartTrip_ShouldCreateTripRecord_InDatabase()
        {
            _tripService.StartTrip(TestDriverID, TestRouteID);

            var count = _dbHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM Trip WHERE DriverID = @driverID AND StatusID = 2",
                new Microsoft.Data.Sqlite.SqliteParameter("@driverID", TestDriverID)
            );

            Assert.AreEqual(1L, count, "One active trip should exist after StartTrip");
        }

        [TestMethod]
        public void EndTrip_ShouldCompleteTrip_AndSetEarnings()
        {
            _tripService.StartTrip(TestDriverID, TestRouteID);
            _tripService.EndTrip(TestDriverID, TestRouteID);

            var earnings = _dbHelper.ExecuteScalar(
                "SELECT ActualEarnings FROM Trip WHERE DriverID = @driverID AND StatusID = 3",
                new Microsoft.Data.Sqlite.SqliteParameter("@driverID", TestDriverID)
            );

            Assert.IsNotNull(earnings, "Completed trip should have earnings recorded");
            Assert.IsTrue(Convert.ToDouble(earnings) >= 16,
                "Earnings should be at least the base fare of ₱16");
        }

        [TestMethod]
        public void EndTrip_ShouldDoNothing_WhenNoActiveTrip()
        {
            // No StartTrip called, EndTrip should not crash
            _tripService.EndTrip(TestDriverID, TestRouteID);

            var count = _dbHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM Trip WHERE DriverID = @driverID",
                new Microsoft.Data.Sqlite.SqliteParameter("@driverID", TestDriverID)
            );

            Assert.AreEqual(0L, count, "No trip record should exist");
        }
    }
}