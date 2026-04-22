using System;
using System.Collections.Generic;
using System.Text;
using TriQue.Models;

namespace TriQue.Services
{
    public class AuthenticationService
    {
        private User _currentUser;

        public User GetCurrentUser() => _currentUser;
        public void Login(string username, string password)
        {

        }
    }
}
