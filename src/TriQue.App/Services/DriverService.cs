using System;
using TriQue.Data.Repositories;
using TriQue.Enums;
using TriQue.Models;

namespace TriQue.Services
{
    public class DriverService
    {
        private readonly DriverRepository _driverRepo;

        public DriverService()
        {
            _driverRepo = new DriverRepository();
        }

        public Driver? GetByUserId(int userID)
        {
            return _driverRepo.GetByUserID(userID);
        }

        public Driver? GetByDriverId(int driverID)
        {
            return _driverRepo.GetByDriverID(driverID);
        }

        public int? GetDriverId(int userID)
        {
            return _driverRepo.GetByUserID(userID)?.DriverID;
        }

        public void UpdateStatus(int driverID, DriverStatus status)
        {
            _driverRepo.UpdateStatus(driverID, (int)status);
        }
    }
}
