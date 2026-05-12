using System;
using TriQue.Data.Repositories;
using TriQue.Models;

namespace TriQue.Services
{
    public class DriverService
    {
        private readonly DriverRepository _driverRepo;

        public Driver? GetByUserId(int userId)
        {
            return _driverRepo.GetByUserID(userId);
        }

        public int? GetDriverId(int userId)
        {
            return _driverRepo.GetByUserID(userId)?.DriverID;
        }

        public bool DriverExists(int userId)
        {
            return _driverRepo.GetByUserID(userId) != null;
        }
    }
}
