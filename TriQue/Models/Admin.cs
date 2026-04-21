using System;
using TriQue.Enums;

namespace TriQue.Models
{
    public class Admin : User
    {
        public required AdminLevel Level { get; set; }

    }
}
