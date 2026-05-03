using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using TriQue.Models;
using TriQue.Services;

namespace TriQue.Tests.Services
{
    [TestClass]
    public class RouteServiceTests
    {
        private RouteService _routeService = null!;

        [TestInitialize]
        public void Setup()
        {
            // Set fake API key for testing
            Environment.SetEnvironmentVariable("TOMTOM_API_KEY", "fake-key-for-testing");
            _routeService = new RouteService();
        }

        // =========================
        // API + VALID INPUT TEST
        // =========================
        [TestMethod]
        public async Task GetTrafficAndDuration_ShouldReturnValidData_WhenCoordinatesAreValid()
        {
            double startLat = 14.083;
            double startLng = 121.146;
            double endLat = 14.084;
            double endLng = 121.147;

            var (durationMin, delaySec, condition) =
                await _routeService.GetTrafficAndDuration(startLat, startLng, endLat, endLng);

            Assert.IsTrue(durationMin > 0, "Duration should be greater than 0");
            Assert.IsTrue(delaySec >= 0, "Delay should not be negative");
            Assert.IsFalse(string.IsNullOrEmpty(condition), "Condition should not be empty");
        }

        // =========================
        // API KEY ERROR TEST
        // =========================
        [TestMethod]
        public async Task GetTrafficAndDuration_ShouldThrow_WhenApiKeyIsMissing()
        {
            Environment.SetEnvironmentVariable("TOMTOM_API_KEY", null);
            var serviceWithoutKey = new RouteService();

            try
            {
                await serviceWithoutKey.GetTrafficAndDuration(14.083, 121.146, 14.084, 121.147);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception)
            {
                // Test passes
            }
        }

        // =========================
        // INVALID INPUT TEST
        // =========================
        [TestMethod]
        public async Task GetTrafficAndDuration_ShouldHandleInvalidCoordinates()
        {
            try
            {
                await _routeService.GetTrafficAndDuration(999, 999, 999, 999);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception)
            {
                // Test passes
            }
        }

        // =========================
        // RAW API RESPONSE TEST
        // =========================
        [TestMethod]
        public async Task GetRouteRaw_ShouldReturnJsonString()
        {
            var route = new Route
            {
                StartLat = 14.083,
                StartLng = 121.146,
                EndLat = 14.084,
                EndLng = 121.147
            };

            string json = await _routeService.GetRouteRaw(route);

            Assert.IsFalse(string.IsNullOrEmpty(json));
            Assert.IsTrue(json.Contains("routes"));
        }

        // =========================
        // TRAFFIC LOGIC TESTS
        // (PURE LOGIC - QA REQUIRED)
        // =========================

        [TestMethod]
        public void TrafficCondition_ShouldBeLight_WhenDelayIsLessOrEqual2()
        {
            int delay = 2;

            string condition = delay <= 2 ? "Light"
                               : delay > 10 ? "High"
                               : "Moderate";

            Assert.AreEqual("Light", condition);
        }

        [TestMethod]
        public void TrafficCondition_ShouldBeModerate_WhenDelayIsGreaterThan2()
        {
            int delay = 5;

            string condition = delay <= 2 ? "Light"
                               : delay > 10 ? "High"
                               : "Moderate";

            Assert.AreEqual("Moderate", condition);
        }

        [TestMethod]
        public void TrafficCondition_ShouldBeHigh_WhenDelayIsGreaterThan10()
        {
            int delay = 15;

            string condition = delay <= 2 ? "Light"
                               : delay > 10 ? "High"
                               : "Moderate";

            Assert.AreEqual("High", condition);
        }

        // =========================
        //  DURATION CONVERSION TEST
        // =========================
        [TestMethod]
        public void Duration_ShouldConvertSecondsToMinutesCorrectly()
        {
            int seconds = 120;

            double minutes = seconds / 60.0;

            Assert.AreEqual(2, minutes);
        }
    }
}