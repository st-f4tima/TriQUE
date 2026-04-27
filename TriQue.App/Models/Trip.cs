using System;
using System.Collections.Generic;
using System.Text;
using TriQue.Enums;

namespace TriQue.Models
{
    public class Trip
    {
        public int TripID { get; set; }
        public int DriverID { get; set; }
        public int RouteID { get; set; }
        public double ActualEarnings { get; set; } = 0;
        public double EarningGoal { get; set; } = 500; // i actually dont know
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DriverStatus Status { get; set; }
    }
}
