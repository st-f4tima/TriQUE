using System;
using System.Collections.Generic;
using System.Text;

namespace TriQue.Models
{
    public class QueueEntry
    {
        public int EntryID { get; set; }
        public int QueueID { get; set; }
        public int DriverID { get; set; }
        public int QueuePosition { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.Now;
    }
}
