using System;
using TriQue.Data.Repositories;
using TriQue.DTOs;
using TriQue.Enums;
using TriQue.Models;

namespace TriQue.Services
{
    public class DriverDashboardService
    {
        private readonly UserRepository _userRepo = new();
        private readonly DriverRepository _driverRepo = new();
        private readonly QueueRepository _queueRepo = new();
        private readonly TripService _tripService = new();
        private readonly RotationService _rotationService = new();

        public Route? GetDriverRouteByDriverID(int driverID)
        {
            var driver = _driverRepo.GetByDriverID(driverID);
            if (driver == null) return null;

            return _rotationService.GetTodayRoute(driver.GroupID);
        }

        public Driver? GetDriver(int userID)
        {
            var user = _userRepo.GetById(userID);
            if (user == null) return null;
            return _driverRepo.GetByUserID(user.UserID);
        }

        public void ResetDriverToWaiting(int driverID)
        {
             _driverRepo.UpdateStatus(driverID, (int)DriverStatus.Waiting);
        }

        public DriverDashboardDto GetDashboard(int userID)
        {
            var user = _userRepo.GetById(userID);
            var driver = _driverRepo.GetByUserID(user.UserID);
            var route = _rotationService.GetTodayRoute(driver.GroupID);
            var stats = _tripService.GetTripSpeedStats(driver.DriverID);

            return new DriverDashboardDto
            {
                User = user,
                Driver = driver,
                Trips = _tripService.GetDriverTrips(driver.DriverID),
                CompletedTrips = _tripService.GetCompletedTrips(driver.DriverID),
                TodayTrips = _tripService.GetTodayTrips(driver.DriverID),
                ActualEarnings = _tripService.GetActualEarnings(driver.DriverID),
                FastestTrip = stats.fastest,
                SlowestTrip = stats.slowest,
                QueueHistory = _queueRepo.GetQueueHistory(driver.DriverID),
                RouteName = route?.RouteName ?? "No Route Today",
                TotalDistance = route?.DistanceKm ?? 0
            };

        }
    }
}
