using System;
using System.Collections.Generic;
using System.Text;
using TriQue.Enums;

namespace TriQue.Models
{
    public class DriverGroup
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public RotationDay GroupRotationDay { get; set; }

    }
}
