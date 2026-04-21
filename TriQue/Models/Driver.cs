using System;
using TriQue.Enums;

namespace TriQue.Models
{
    public class Driver : User
    {
        public int DriverID { get; set; }
        public required int GroupID { get; set; }
        public required string BodyNumber { get; set; }
        public required DriverStatus Status { get; set; } = DriverStatus.Waiting;

    }
}
