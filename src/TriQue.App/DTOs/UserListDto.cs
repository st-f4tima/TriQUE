using System;
using System.Collections.Generic;
using System.Text;

namespace TriQue.DTOs
{
    public class UserListDto
    {
        public int UserID { get; set; }
        public string FullName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string RoleName { get; set; } = "";
        public string AssignedRoute { get; set; } = "—";
        public string Status { get; set; } = "Active";
    }
}