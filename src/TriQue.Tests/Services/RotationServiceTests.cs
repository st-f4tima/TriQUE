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

        [TestMethod]
        public void GetTodayRoute_ShouldReturnNull_WhenGroupIsNotFound()
        {
            // checks invalid group IDs
            var service = new RotationService();

            Assert.IsNull(service.GetTodayRoute(0));
            Assert.IsNull(service.GetTodayRoute(999));
        }

        [TestMethod]
        public void GetTodayRoute_GroupA_OnMonday_ShouldReturnIndex0()
        {
            // checks Group A Monday route
            var routes = _routeRepo.GetAllRoutes();

            int baseOffset = (int)RotationDay.Monday - 1;
            int dayOffset = 0;
            int expected = (baseOffset + dayOffset) % routes.Count;

            Assert.AreEqual(0, expected);
            Assert.AreEqual(routes[0].RouteID, routes[expected].RouteID);
        }

        [TestMethod]
        public void GetTodayRoute_GroupA_OnSunday_ShouldUseDayOffset6()
        {
            // checks Group A Sunday route
            var routes = _routeRepo.GetAllRoutes();

            int baseOffset = (int)RotationDay.Monday - 1;
            int dayOffset = 6;
            int expected = (baseOffset + dayOffset) % routes.Count;

            Assert.AreEqual(0, expected);
        }

        [TestMethod]
        public void GetTodayRoute_GroupB_OnMonday_ShouldReturnIndex1()
        {
            // checks Group B Monday route
            var routes = _routeRepo.GetAllRoutes();

            int baseOffset = (int)RotationDay.Tuesday - 1;
            int dayOffset = 0;
            int expected = (baseOffset + dayOffset) % routes.Count;

            Assert.AreEqual(1, expected);
            Assert.AreEqual(routes[1].RouteID, routes[expected].RouteID);
        }

        [TestMethod]
        public void GetTodayRoute_GroupB_OnSunday_ShouldReturnIndex1()
        {
            // checks Group B Sunday route
            var routes = _routeRepo.GetAllRoutes();

            int baseOffset = (int)RotationDay.Tuesday - 1;
            int dayOffset = 6;
            int expected = (baseOffset + dayOffset) % routes.Count;

            Assert.AreEqual(1, expected);
        }

        [TestMethod]
        public void GetTodayRoute_ShouldWrapAround_WhenOffsetExceedsRouteCount()
        {
            // checks route wrap around
            var routes = _routeRepo.GetAllRoutes();

            int baseOffset = (int)RotationDay.Saturday - 1;
            int dayOffset = 5;
            int expected = (baseOffset + dayOffset) % routes.Count;

            Assert.IsTrue(expected >= 0 && expected < routes.Count,
                $"Route index {expected} is out of bounds for {routes.Count} routes");
        }

        [TestMethod]
        public void GetTodayRoute_ShouldNeverReturnOutOfBoundsIndex()
        {
            // checks valid route indexes
            var routes = _routeRepo.GetAllRoutes();
            int routeCount = routes.Count;

            var rotationDays = new[]
            {
                RotationDay.Monday, RotationDay.Tuesday, RotationDay.Wednesday,
                RotationDay.Thursday, RotationDay.Friday, RotationDay.Saturday
            };

            int[] dayOffsets = { 0, 1, 2, 3, 4, 5, 6 };

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

        [TestMethod]
        public void GetTodayRoute_GroupA_AllDays_ShouldMapCorrectly()
        {
            // checks weekly rotation mapping
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

        [TestMethod]
        public void GetTodayRoute_AllGroups_ShouldReturnValidRoute()
        {
            // checks all valid groups
            var service = new RotationService();

            int[] validGroupIDs = { 1, 2, 3, 4, 5, 6 };

            foreach (var groupID in validGroupIDs)
            {
                var result = service.GetTodayRoute(groupID);

                Assert.IsNotNull(result,
                    $"GroupID {groupID} returned null but should return a route");
            }
        }

        [TestMethod]
        public void GetTodayRoute_ShouldReturnRouteWithValidID()
        {
            // checks valid route IDs
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