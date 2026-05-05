// File: TriQue/Models/UserListItem.cs
// Add these small DTO classes to your Models folder

namespace TriQue.DTOs
{
    public class UserListItem
    {
        public int UserID { get; set; }
        public string FullName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string RoleName { get; set; } = "";
        public string AssignedRoute { get; set; } = "—";
        public string Status { get; set; } = "Active";
    }

    public class UserDetailItem
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

    public class RouteItem
    {
        public int RouteID { get; set; }
        public string RouteName { get; set; } = "";
        public override string ToString() => RouteName;
    }

    public class CreatedUserDTO
    {
        public string Username { get; set; } = "";
        public string TempPassword { get; set; } = "";
    }
}