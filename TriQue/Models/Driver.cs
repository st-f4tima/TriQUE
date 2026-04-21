using System;
using TriQue.Enums;

namespace TriQue.Models
{
    public class Driver : User
    {
        public int DriverID { get; set; }
        public int GroupID { get; set; }
        public string BodyNumber { get; set; } = string.Empty;
        public DriverStatus Status { get; set; } = DriverStatus.Waiting;

    }
}
