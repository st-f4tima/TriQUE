using System;
using TriQue.Data.Repositories;
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

        public Driver GetDriverByUserID(int userID)
        {
            return _driverRepo.GetByUserID(userID);
        }
    }
}
