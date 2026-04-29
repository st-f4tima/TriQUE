using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TriQue.Models;

namespace TriQue.Services
{
    public class RouteService
    {
        private readonly string _apiKey;

        public RouteService()
        {
            _apiKey = Environment.GetEnvironmentVariable("TOMTOM_API_KEY");

            if (string.IsNullOrEmpty(_apiKey))
                throw new Exception("TOMTOM_API_KEY is missing");
        }

        public async Task<string> GetRouteRaw(Route route)
        {
            string url =
                $"https://api.tomtom.com/routing/1/calculateRoute/" +
                $"{route.StartLat},{route.StartLng}:" +
                $"{route.EndLat},{route.EndLng}/json?key={_apiKey}";

            using HttpClient client = new HttpClient();
            return await client.GetStringAsync(url);
        }

        public async Task<(double durationMin, string trafficCondition)> GetTrafficAndDuration(
            double startLat, double startLng,
            double endLat, double endLng)
        {
            string key = Environment.GetEnvironmentVariable("TOMTOM_API_KEY");

            string url =
                $"https://api.tomtom.com/routing/1/calculateRoute/" +
                $"{startLat},{startLng}:{endLat},{endLng}/json?key={key}";

            using var client = new HttpClient();
            var response = await client.GetStringAsync(url);

            using var doc = JsonDocument.Parse(response);

            var summary = doc.RootElement
                .GetProperty("routes")[0]
                .GetProperty("summary");

            double durationMin = summary
                .GetProperty("travelTimeInSeconds").GetDouble() / 60;

            double delayMin = summary
                .GetProperty("trafficDelayInSeconds").GetDouble() / 60;

            // classify traffic
            string trafficCondition = "Light";

            if (delayMin > 10)
                trafficCondition = "Heavy";
            else if (delayMin > 2)
                trafficCondition = "Moderate";

            return (Math.Round(durationMin, 1), trafficCondition);
        }
    }
}