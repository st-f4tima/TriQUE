using System;
using System.Collections.Generic;
using System.Text;

namespace TriQue.Models
{
    public class Route
    {
        public int RouteID { get; set; }
        public string RouteName { get; set; } = string.Empty;
        public int AssignedGroup { get; set; }

        public double StartLat { get; set; }
        public double StartLng { get; set; }
        public double EndLat { get; set; }
        public double EndLng { get; set; }

    }
}
