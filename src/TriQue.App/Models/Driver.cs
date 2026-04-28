using System;
using TriQue.Enums;
using TriQue.Forms;

namespace TriQue.Models
{
    public class Driver : User
    {
        public int DriverID { get; set; }
        public int GroupID { get; set; }
        public string BodyNumber { get; set; } = string.Empty;
        public DriverStatus Status { get; set; } = DriverStatus.Waiting;
        public override Form GetView()
        {
            return new DriverForm(UserID);
        }
    }
}
