using TriQue.Data.Repositories;
using TriQue.Models;

namespace TriQue.Services
{
    public class RotationService
    {
        private readonly RouteRepository _routeRepo;
        private readonly DriverRepository _driverRepo;

        public RotationService()
        {
            _routeRepo = new RouteRepository();
            _driverRepo = new DriverRepository();
        }

        public Route? GetTodayRoute(int groupID)
        {
            var routes = _routeRepo.GetAllRoutes();
            var group = _driverRepo.GetGroupByID(groupID);

            if (routes.Count == 0 || group == null) return null;

            // Sun=0, Mon=1, Tue=2, Wed=3, Thu=4, Fri=5, Sat=6
            int today = (int)DateTime.Today.DayOfWeek; 

            int dayOffset;
            if (today == 0)
            {
                dayOffset = 6; // Sunday
            }
            else
            {
                dayOffset = today - 1; // Mon=0, Tue=1, Wed=2...
            }

            int baseOffset = (int)group.GroupRotationDay - 1;
            // explantion:
            // Group A → Monday(1) - 1 = 0  → starts at index 0
            // Group B → Tuesday(2) - 1 = 1 → starts at index 1
            // Group C → Wednesday(3) - 1 = 2 → starts at index 2

            int routeIndex = (baseOffset + dayOffset) % routes.Count;
            return routes[routeIndex];


        }
    }
}