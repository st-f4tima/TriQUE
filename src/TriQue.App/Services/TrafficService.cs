using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TriQue.Data.Repositories;
using TriQue.DTOs;

namespace TriQue.Services
{
    public class TrafficService
    {
        private readonly string _apiKey;
        private readonly RouteRepository _routeRepo;
        private readonly TrafficRepository _trafficRepo;
        private readonly RouteService _routeService;

        public TrafficService()
        {
            _apiKey = Environment.GetEnvironmentVariable("TOMTOM_API_KEY")
                           ?? throw new Exception("TOMTOM_API_KEY is missing");
            _routeRepo = new RouteRepository();
            _trafficRepo = new TrafficRepository();
            _routeService = new RouteService();
        }
        
        public async Task<List<TrafficDto>> GetAllRouteTrafficAsync()
        {
            int[] routeIds = { 101, 102, 103, 104, 105, 106 };
            var results = new List<TrafficDto>();

            foreach (var id in routeIds)
            {
                var route = _routeRepo.GetRouteByID(id);
                if (route == null) continue;

                try
                {
                    var (durationMin, delaySec, condition) =
                        await _routeService.GetTrafficAndDuration(
                            route.StartLat,
                            route.StartLng,
                            route.EndLat,
                            route.EndLng);

                    _trafficRepo.SaveTrafficLog(route.RouteID, delaySec, condition);

                    results.Add(new TrafficDto
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
                    results.Add(new TrafficDto
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