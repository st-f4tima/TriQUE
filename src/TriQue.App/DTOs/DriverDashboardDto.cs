using System.Collections.Generic;
using System.Data;
using TriQue.Models;

namespace TriQue.DTOs
{
    public class DriverDashboardDto
    {
        public User? User { get; set; }
        public Driver? Driver { get; set; }
        public List<Trip>? Trips { get; set; }

        public int CompletedTrips { get; set; }
        public int TodayTrips { get; set; }

        public double ActualEarnings { get; set; }
        public double GoalEarnings { get; set; }
        public double FastestTrip { get; set; }
        public double SlowestTrip { get; set; }
        public DataTable QueueHistory { get; set; }
        public string RouteName { get; set; } 
        public double TotalDistance { get; set; }
    }
}