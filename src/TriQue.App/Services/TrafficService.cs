using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TriQue.Data.Repositories;
using TriQue.Models;

namespace TriQue.Services
{
    public class TrafficService
    {
        private readonly string _apiKey;
        private readonly RouteRepository _routeRepo;
        private readonly TrafficRepository _trafficRepo;

        public TrafficService()
        {
            _apiKey = Environment.GetEnvironmentVariable("TOMTOM_API_KEY")
                           ?? throw new Exception("TOMTOM_API_KEY is missing");
            _routeRepo = new RouteRepository();
            _trafficRepo = new TrafficRepository();
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

            string level;

            if (delaySec <= 120)
            {
                level = "Light";
            }
            else if (delaySec <= 600)
            {
                level = "Moderate";
            }
            else
            {
                level = "Heavy";
            }

            return (Math.Round(durationSec / 60, 1), delaySec, level);
        }

        public async Task<List<TrafficData>> GetAllRouteTrafficAsync()
        {
            int[] routeIds = { 101, 102, 103, 104, 105, 106 };
            var results = new List<TrafficData>();

            foreach (var id in routeIds)
            {
                var route = _routeRepo.GetRouteByID(id);
                if (route == null) continue;

                try
                {
                    var (durationMin, delaySec, condition) =
                        await GetTrafficAndDuration(
                            route.StartLat, route.StartLng,
                            route.EndLat, route.EndLng);

                    _trafficRepo.SaveTrafficLog(route.RouteID, delaySec, condition);

                    results.Add(new TrafficData
                    {
                        RouteID = route.RouteID,
                        RouteName = route.RouteName,
                        DurationMin = durationMin,
                        DelaySec = delaySec,
                        TrafficCondition = condition,
                        IsTrafficProne = _trafficRepo.IsTrafficProne(route.RouteID),
                        PeakWindow = _trafficRepo.GetPeakWindow(route.RouteID)
                    });
                }
                catch
                {
                    results.Add(new TrafficData
                    {
                        RouteID = route.RouteID,
                        RouteName = route.RouteName,
                        TrafficCondition = "Unknown",
                        PeakWindow = "No Data Yet",
                        IsTrafficProne = false
                    });
                }
            }

            return results;
        }
    }
}