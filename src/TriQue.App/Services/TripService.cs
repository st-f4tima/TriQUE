using System;
using System.Collections.Generic;
using TriQue.Data.Repositories;
using TriQue.Models;

namespace TriQue.Services
{
    public class TripService
    {
        private readonly TripRepository _tripRepo;
        private readonly RouteRepository _routeRepo;

        public TripService()
        {
            _tripRepo = new TripRepository();
            _routeRepo = new RouteRepository();
        }

        public List<Trip> GetDriverTrips(int driverID)
        {
            return _tripRepo.GetByDriverID(driverID);
        }

        public int GetCompletedTrips(int driverID)
        {
            return _tripRepo.GetCompletedTrips(driverID);
        }

        public int GetTodayTrips(int driverID)
        {
            return _tripRepo.GetTodayTrips(driverID);
        }

        public double GetActualEarnings(int driverID)
        {
            return _tripRepo.GetEarningsProgress(driverID);
        }

        public (double fastest, double slowest) GetTripSpeedStats(int driverID)
        {
            return _tripRepo.GetTripSpeedStats(driverID);
        }

        public void StartTrip(int driverID, int routeID)
        {
            _tripRepo.StartTrip(driverID, routeID);
        }

        public void EndTrip(int driverID, int routeID)
        {
            int? tripID = _tripRepo.GetActiveTripID(driverID);
            if (tripID == null) return;

            double fare = CalculateFare(routeID);
            _tripRepo.EndTrip(tripID.Value, fare);
        }

        // LTFRB tricycle fare formula:
        // ₱16 for first km, ₱5 per succeeding 500m
        private double CalculateFare(int routeID)
        {
            var route = _routeRepo.GetRouteByID(routeID);
            if (route == null) return 16;

            double distanceKm = route.DistanceKm;

            if (distanceKm <= 1.0)
                return 16;

            double succeedingKm = distanceKm - 1.0;
            double succeedingFare = Math.Ceiling(succeedingKm / 0.5) * 5;

            return 16 + succeedingFare;
        }
    }
}