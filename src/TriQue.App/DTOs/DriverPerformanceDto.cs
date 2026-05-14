using System;
using System.Collections.Generic;
using System.Text;

namespace TriQue.DTOs
{
    public class DriverPerformanceDto
    {
        public int DriverID { get; set; }
        public string FullName { get; set; } = "";
        public string BodyNumber { get; set; } = "";
        public string GroupName { get; set; } = "";
        public int TotalTrips { get; set; }
        public int CompletedTrips { get; set; }
        public double TotalEarnings { get; set; }
        public double AvgDuration { get; set; }
        public double FastestTrip { get; set; }
        public double SlowestTrip { get; set; }
    }
}
