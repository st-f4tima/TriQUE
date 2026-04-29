using System;
using TriQue.Data.Repositories;
using TriQue.Models;

namespace TriQue.Services
{
    public class DriverDashboardService
    {
        private readonly UserRepository _userRepo;
        private readonly DriverRepository _driverRepo;
        private readonly TripRepository _tripRepo;
        private readonly QueueRepository _queueRepo;
        private readonly RouteService _routeService;
        private readonly RouteRepository _routeRepo;

        public DriverDashboardService()
        {
            _userRepo = new UserRepository();
            _driverRepo = new DriverRepository();
            _tripRepo = new TripRepository();
            _queueRepo = new QueueRepository();
            _routeRepo = new RouteRepository();
            _routeService = new RouteService();
        }

        public Route? GetDriverRouteByDriverID(int driverID)
        {
            var driver = _driverRepo.GetByDriverID(driverID);
            if (driver == null) return null;

            return _routeRepo.GetRouteByGroupID(driver.GroupID);
        }

        public Driver? GetDriver(int userID)
        {
            var user = _userRepo.GetById(userID);
            if (user == null) return null;
            return _driverRepo.GetByUserID(user.UserID);
        }

        public DriverDashboardData GetDashboard(int userID)
        {
            var user = _userRepo.GetById(userID);
            var driver = _driverRepo.GetByUserID(user.UserID);

            var trips = _tripRepo.GetByDriverID(driver.DriverID);
            var completedTrips = _tripRepo.GetCompletedTrips(driver.DriverID);
            var todayTrips = _tripRepo.GetTodayTrips(driver.DriverID);
            var actualEarnings = _tripRepo.GetEarningsProgress(driver.DriverID);


            var stats = _tripRepo.GetTripSpeedStats(driver.DriverID);
            var queueHistory = _queueRepo.GetQueueHistory(driver.DriverID);


            return new DriverDashboardData
            {
                User = user,
                Driver = driver,
                Trips = trips,
                CompletedTrips = completedTrips,
                TodayTrips = todayTrips,
                ActualEarnings = actualEarnings,
                FastestTrip = stats.fastest,
                SlowestTrip = stats.slowest,
                QueueHistory = queueHistory
            };

        }
    }
}
