using System;
using System.Net.Http;
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
    }
}