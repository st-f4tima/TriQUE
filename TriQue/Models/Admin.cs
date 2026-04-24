using System;
using TriQue.Enums;

namespace TriQue.Models
{
    public class Admin : User
    {
        public int AdminID { get; set; }
        public AdminLevel Level { get; set; }

    }
}
