using System;
using TriQue.Enums;

namespace TriQue.Models
{
    public class Admin : User
    {
        public AdminLevel Level { get; set; }

    }
}
