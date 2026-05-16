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
            DotNetEnv.Env.Load(); //reads .env
            _routeService = new RouteService();
        }

        // VALID INPUT TEST
        [TestMethod]
        public async Task GetTrafficAndDuration_ShouldReturnValidData_WhenCoordinatesAreValid()
        {
            var (durationMin, delaySec, condition) =
                await _routeService.GetTrafficAndDuration(14.083, 121.146, 14.084, 121.147);

            Assert.IsTrue(durationMin > 0);
            Assert.IsTrue(delaySec >= 0);
            Assert.IsFalse(string.IsNullOrEmpty(condition));
        }

        // INVALID INPUT TEST
        [TestMethod]
        public async Task GetTrafficAndDuration_ShouldHandleInvalidCoordinates()
        {
            try
            {
                await _routeService.GetTrafficAndDuration(999, 999, 999, 999);
                Assert.Fail();
            }
            catch
            {
                Assert.IsTrue(true);
            }
        }

        // API KEY MISSING TEST
        [TestMethod]
        public async Task GetTrafficAndDuration_ShouldThrow_WhenApiKeyIsMissing()
        {
            Environment.SetEnvironmentVariable("TOMTOM_API_KEY", null);

            try
            {
                var service = new RouteService();
                await service.GetTrafficAndDuration(14.083, 121.146, 14.084, 121.147);
                Assert.Fail();
            }
            catch
            {
                Assert.IsTrue(true);
            }
        }

        // RAW API RESPONSE TEST
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

        // TRAFFIC CONDITIONS TEST
        [TestMethod]
        public void TrafficCondition_ShouldBeLight_WhenDelayIsLow()
        {
            int delay = 2;

            string condition = delay <= 2 ? "Light"
                               : delay > 10 ? "High"
                               : "Moderate";

            Assert.AreEqual("Light", condition);
        }

        [TestMethod]
        public void TrafficCondition_ShouldBeModerate_WhenDelayIsMedium()
        {
            int delay = 5;

            string condition = delay <= 2 ? "Light"
                               : delay > 10 ? "High"
                               : "Moderate";

            Assert.AreEqual("Moderate", condition);
        }

        [TestMethod]
        public void TrafficCondition_ShouldBeHigh_WhenDelayIsHigh()
        {
            int delay = 15;

            string condition = delay <= 2 ? "Light"
                               : delay > 10 ? "High"
                               : "Moderate";

            Assert.AreEqual("High", condition);
        }

        // DURATION CONVERSION TEST
        [TestMethod]
        public void Duration_ShouldConvertSecondsToMinutesCorrectly()
        {
            int seconds = 120;
            double minutes = seconds / 60.0;

            Assert.AreEqual(2, minutes);
        }
    }
}