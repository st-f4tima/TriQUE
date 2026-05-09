using Microsoft.VisualStudio.TestTools.UnitTesting;
using TriQue.Data.Database;
using TriQue.Helpers;
using TriQue.Services;
using TriQue.Models;
using TriQue.Enums;
using TriQue.Data.Repositories;

namespace TriQue.Tests.Services
{
    [TestClass]
    public class RotationServiceTests
    {
        private DatabaseHelper _dbHelper;
        private RouteRepository _routeRepo;

        [TestInitialize]
        public void Setup()
        {
            _dbHelper = new DatabaseHelper();
            var dbInitializer = new DatabaseInitializer(_dbHelper, AppConfig.Configuration);
            dbInitializer.Initialize();
            _routeRepo = new RouteRepository();
        }

        // =============================================
        // NULL / EMPTY CASES
        // =============================================

        [TestMethod]
        public void GetTodayRoute_ShouldReturnNull_WhenGroupIsNotFound()
        {
            // GroupID 0 and 999 do not exist in the database
            var service = new RotationService();

            Assert.IsNull(service.GetTodayRoute(0));
            Assert.IsNull(service.GetTodayRoute(999));
        }

        // =============================================
        // GROUP A TESTS
        // =============================================

        [TestMethod]
        public void GetTodayRoute_GroupA_OnMonday_ShouldReturnIndex0()
        {
            // Group A has RotationDay = Monday (1), baseOffset = 0
            // Monday dayOffset = 0
            // routeIndex = (0 + 0) % 6 = 0 -> first route
            var routes = _routeRepo.GetAllRoutes();

            int baseOffset = (int)RotationDay.Monday - 1; // 0
            int dayOffset = 0; // Monday
            int expected = (baseOffset + dayOffset) % routes.Count;

            Assert.AreEqual(0, expected);
            Assert.AreEqual(routes[0].RouteID, routes[expected].RouteID);
        }

        [TestMethod]
        public void GetTodayRoute_GroupA_OnSunday_ShouldUseDayOffset6()
        {
            // Group A has RotationDay = Monday (1), baseOffset = 0
            // Sunday dayOffset = 6
            // routeIndex = (0 + 6) % 6 = 0 → wraps back to first route
            var routes = _routeRepo.GetAllRoutes();

            int baseOffset = (int)RotationDay.Monday - 1; // 0
            int dayOffset = 6; // Sunday
            int expected = (baseOffset + dayOffset) % routes.Count;

            Assert.AreEqual(0, expected);
        }

        // =============================================
        // GROUP B TESTS
        // =============================================

        [TestMethod]
        public void GetTodayRoute_GroupB_OnMonday_ShouldReturnIndex1()
        {
            // Group B has RotationDay = Tuesday (2), baseOffset = 1
            // Monday dayOffset = 0
            // routeIndex = (1 + 0) % 6 = 1 -> second route
            var routes = _routeRepo.GetAllRoutes();

            int baseOffset = (int)RotationDay.Tuesday - 1; // 1
            int dayOffset = 0; // Monday
            int expected = (baseOffset + dayOffset) % routes.Count;

            Assert.AreEqual(1, expected);
            Assert.AreEqual(routes[1].RouteID, routes[expected].RouteID);
        }

        [TestMethod]
        public void GetTodayRoute_GroupB_OnSunday_ShouldReturnIndex1()
        {
            // Group B has RotationDay = Tuesday (2), baseOffset = 1
            // Sunday dayOffset = 6
            // routeIndex = (1 + 6) % 6 = 1 -> second route
            var routes = _routeRepo.GetAllRoutes();

            int baseOffset = (int)RotationDay.Tuesday - 1; // 1
            int dayOffset = 6; // Sunday
            int expected = (baseOffset + dayOffset) % routes.Count;

            Assert.AreEqual(1, expected);
        }

        // =============================================
        // WRAP AROUND TEST
        // =============================================

        [TestMethod]
        public void GetTodayRoute_ShouldWrapAround_WhenOffsetExceedsRouteCount()
        {
            // Group F has RotationDay = Saturday (6), baseOffset = 5
            // Saturday dayOffset = 5
            // routeIndex = (5 + 5) % 6 = 4 -> wraps correctly
            var routes = _routeRepo.GetAllRoutes();

            int baseOffset = (int)RotationDay.Saturday - 1; // 5
            int dayOffset = 5; // Saturday
            int expected = (baseOffset + dayOffset) % routes.Count;

            Assert.IsTrue(expected >= 0 && expected < routes.Count,
                $"Route index {expected} is out of bounds for {routes.Count} routes");
        }

        [TestMethod]
        public void GetTodayRoute_ShouldNeverReturnOutOfBoundsIndex()
        {
            // Test all 6 groups across all 7 days — index must always be valid
            var routes = _routeRepo.GetAllRoutes();
            int routeCount = routes.Count;

            var rotationDays = new[]
            {
                RotationDay.Monday, RotationDay.Tuesday, RotationDay.Wednesday,
                RotationDay.Thursday, RotationDay.Friday, RotationDay.Saturday
            };

            int[] dayOffsets = { 0, 1, 2, 3, 4, 5, 6 }; // Mon to Sun

            foreach (var rotDay in rotationDays)
            {
                int baseOffset = (int)rotDay - 1;

                foreach (var dayOffset in dayOffsets)
                {
                    int index = (baseOffset + dayOffset) % routeCount;

                    Assert.IsTrue(index >= 0 && index < routeCount,
                        $"Index {index} out of bounds for rotationDay={rotDay}, dayOffset={dayOffset}");
                }
            }
        }

        // =============================================
        // ALL DAYS OF THE WEEK - GROUP A
        // =============================================

        [TestMethod]
        public void GetTodayRoute_GroupA_AllDays_ShouldMapCorrectly()
        {
            // Group A baseOffset = 0
            // Day offsets: Mon=0, Tue=1, Wed=2, Thu=3, Fri=4, Sat=5, Sun=6
            // Expected indices: 0, 1, 2, 3, 4, 5, 0
            var routes = _routeRepo.GetAllRoutes();
            int baseOffset = 0;
            int[] dayOffsets = { 0, 1, 2, 3, 4, 5, 6 };
            int[] expectedIndices = { 0, 1, 2, 3, 4, 5, 0 };

            for (int i = 0; i < dayOffsets.Length; i++)
            {
                int actual = (baseOffset + dayOffsets[i]) % routes.Count;
                Assert.AreEqual(expectedIndices[i], actual,
                    $"Group A on dayOffset={dayOffsets[i]}: expected index {expectedIndices[i]}, got {actual}");
            }
        }

        // =============================================
        // FULL ROTATION CORRECTNESS
        // =============================================

        [TestMethod]
        public void GetTodayRoute_AllGroups_ShouldReturnValidRoute()
        {
            // All 6 valid group IDs should return a non-null route
            var service = new RotationService();
            int[] validGroupIDs = { 1, 2, 3, 4, 5, 6 };

            foreach (var groupID in validGroupIDs)
            {
                var result = service.GetTodayRoute(groupID);
                Assert.IsNotNull(result, $"GroupID {groupID} returned null but should return a route");
            }
        }

        [TestMethod]
        public void GetTodayRoute_ShouldReturnRouteWithValidID()
        {
            // Route returned should have a valid RouteID (101-106)
            var service = new RotationService();
            int[] validRouteIDs = { 101, 102, 103, 104, 105, 106 };

            for (int groupID = 1; groupID <= 6; groupID++)
            {
                var result = service.GetTodayRoute(groupID);
                Assert.IsNotNull(result);
                CollectionAssert.Contains(validRouteIDs, result.RouteID,
                    $"GroupID {groupID} returned unexpected RouteID {result?.RouteID}");
            }
        }
    }
}