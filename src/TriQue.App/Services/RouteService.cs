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

        public async Task<(double durationMin, double delaySec, string trafficCondition)>
    GetTrafficAndDuration(double startLat, double startLng,
                          double endLat, double endLng)
        {
            string url =
                $"https://api.tomtom.com/routing/1/calculateRoute/" +
                $"{startLat},{startLng}:{endLat},{endLng}/json" +
                $"?traffic=true&key={_apiKey}";   

            using var client = new HttpClient();
            var response = await client.GetStringAsync(url);

            using var doc = JsonDocument.Parse(response);

            var summary = doc.RootElement
                .GetProperty("routes")[0]
                .GetProperty("summary");

            double durationSec = summary.GetProperty("travelTimeInSeconds").GetDouble();
            double delaySec = summary.GetProperty("trafficDelayInSeconds").GetDouble();

            double delayRatio = durationSec > 0 ? delaySec / durationSec : 0;

            string trafficCondition = delayRatio >= 0.20 ? "Heavy"
                         : delayRatio >= 0.05 ? "Moderate"
                         : "Light";

            return (Math.Round(durationSec / 60, 1), delaySec, trafficCondition);
        }
    }
}