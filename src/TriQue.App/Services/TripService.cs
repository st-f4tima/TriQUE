using System;
using TriQue.Data.Repositories;
using TriQue.Models;

namespace TriQue.Services
{
    public class TripService
    {
        private readonly TripRepository _tripRepo;

        public TripService()
        {
            _tripRepo = new TripRepository();
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
    }
}