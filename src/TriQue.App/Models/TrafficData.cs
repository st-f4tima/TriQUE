using System;
using System.Collections.Generic;
using System.Text;

namespace TriQue.Models
{
    public class TrafficData
    {
        public int RouteID { get; set; }
        public string RouteName { get; set; } = "";
        public double DurationMin { get; set; }
        public double DelaySec { get; set; }
        public string TrafficCondition { get; set; } = "Unknown";
        public string PeakWindow { get; set; } = "No Data";
        public bool IsTrafficProne { get; set; }
    }
}