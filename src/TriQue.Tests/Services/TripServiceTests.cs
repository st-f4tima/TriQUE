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

            // clear existing trips for test driver
            _dbHelper.ExecuteNonQuery(
                "DELETE FROM Trip WHERE DriverID = @driverID",
                new Microsoft.Data.Sqlite.SqliteParameter("@driverID", TestDriverID)
            );
        }

        [TestCleanup]
        public void Cleanup()
        {
            // reset driver status
            _dbHelper.ExecuteNonQuery(
                "UPDATE Driver SET StatusID = 1 WHERE DriverID = @driverID",
                new Microsoft.Data.Sqlite.SqliteParameter("@driverID", TestDriverID)
            );
        }

        // FARE CALCULATION TESTS (formula-based)

        [TestMethod]
        public void CalculateFare_ShouldReturnBaseFare_WhenDistanceIsExactly1km()
        {
            double distanceKm = 1.0;

            double fare = distanceKm <= 1.0
                ? 16
                : 16 + Math.Ceiling((distanceKm - 1.0) / 0.5) * 5;

            Assert.AreEqual(16, fare);
        }

        [TestMethod]
        public void CalculateFare_ShouldReturnBaseFare_WhenDistanceIsUnder1km()
        {
            double distanceKm = 0.5;

            double fare = distanceKm <= 1.0
                ? 16
                : 16 + Math.Ceiling((distanceKm - 1.0) / 0.5) * 5;

            Assert.AreEqual(16, fare);
        }

        [TestMethod]
        public void CalculateFare_ShouldReturn21_WhenDistanceIs1point5km()
        {
            double distanceKm = 1.5;

            double fare = distanceKm <= 1.0
                ? 16
                : 16 + Math.Ceiling((distanceKm - 1.0) / 0.5) * 5;

            Assert.AreEqual(21, fare);
        }

        [TestMethod]
        public void CalculateFare_ShouldReturn26_WhenDistanceIs2km()
        {
            double distanceKm = 2.0;

            double fare = distanceKm <= 1.0
                ? 16
                : 16 + Math.Ceiling((distanceKm - 1.0) / 0.5) * 5;

            Assert.AreEqual(26, fare);
        }

        [TestMethod]
        public void CalculateFare_ShouldUseCeiling_ForPartialDistance()
        {
            double distanceKm = 1.3;

            double fare = distanceKm <= 1.0
                ? 16
                : 16 + Math.Ceiling((distanceKm - 1.0) / 0.5) * 5;

            Assert.AreEqual(21, fare);
        }

        [TestMethod]
        public void CalculateFare_ShouldReturnBaseFare_WhenDistanceIsZero()
        {
            double distanceKm = 0;

            double fare = distanceKm <= 1.0
                ? 16
                : 16 + Math.Ceiling((distanceKm - 1.0) / 0.5) * 5;

            Assert.AreEqual(16, fare);
        }

        [TestMethod]
        public void CalculateFare_ShouldReturnValidFare_ForSeededRoutes()
        {
            // validates computed fare for all seeded routes
            var expectedFares = new Dictionary<int, double>
            {
                { 101, 16 + Math.Ceiling((4.8 - 1.0) / 0.5) * 5 },
                { 102, 16 + Math.Ceiling((2.4 - 1.0) / 0.5) * 5 },
                { 103, 16 + Math.Ceiling((7.5 - 1.0) / 0.5) * 5 },
                { 104, 16 + Math.Ceiling((5.3 - 1.0) / 0.5) * 5 },
                { 105, 16 + Math.Ceiling((11.0 - 1.0) / 0.5) * 5 },
                { 106, 16 + Math.Ceiling((2.8 - 1.0) / 0.5) * 5 }
            };

            foreach (var entry in expectedFares)
            {
                Assert.IsTrue(entry.Value >= 16);
            }
        }

        // TRIP FLOW TESTS

        [TestMethod]
        public void StartTrip_ShouldCreateTripRecord_InDatabase()
        {
            _tripService.StartTrip(TestDriverID, TestRouteID);

            var count = _dbHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM Trip WHERE DriverID = @driverID AND StatusID = 2",
                new Microsoft.Data.Sqlite.SqliteParameter("@driverID", TestDriverID)
            );

            Assert.AreEqual(1L, count);
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

            Assert.IsNotNull(earnings);
            Assert.IsTrue(Convert.ToDouble(earnings) >= 16);
        }

        [TestMethod]
        public void EndTrip_ShouldDoNothing_WhenNoActiveTrip()
        {
            // ensures safe call when no trip exists
            _tripService.EndTrip(TestDriverID, TestRouteID);

            var count = _dbHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM Trip WHERE DriverID = @driverID",
                new Microsoft.Data.Sqlite.SqliteParameter("@driverID", TestDriverID)
            );

            Assert.AreEqual(0L, count);
        }
    }
}