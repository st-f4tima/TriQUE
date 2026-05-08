using System;
using System.Collections.Generic;
using System.Text;

namespace TriQue.DTOs
{
    public class UserDetailDto
    {
        public int UserID { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string FullName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string RoleName { get; set; } = "";
        public int RoleID { get; set; }
        public string BodyNumber { get; set; } = "";
        public string AssignedRoute { get; set; } = "—";
        public int RouteID { get; set; }
        public string Status { get; set; } = "Active";
    }
}
