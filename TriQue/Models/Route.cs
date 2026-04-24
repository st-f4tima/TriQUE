using System;
using System.Collections.Generic;
using System.Text;

namespace TriQue.Models
{
    public class Route
    {
        public int RouteID { get; set; }
        public string RouteName { get; set; } = string.Empty;
        public int AssignedGroupID { get; set; }

    }
}
