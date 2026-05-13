using System;
using System.Collections.Generic;
using System.Text;
using TriQue.Data.Repositories;
using TriQue.Enums;

namespace TriQue.Services
{
    public class AdminService
    {
        private readonly AdminRepository _adminRepo = new();

        public AdminLevel GetAdminLevel(int userID)
        {
            return _adminRepo.GetAdminLevel(userID);
        }
    }
}
