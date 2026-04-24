using System;
using System.Collections.Generic;
using System.Text;

namespace TriQue.Models
{
    public class Queue
    {
        public int QueueID { get; set; }
        public int RouteID { get; set; }
        public int CurrentSize { get; set; } = 0;

        public void AddToQueue()
        {
            CurrentSize++;
        }

        public void RemoveToQueue()
        {
            if (CurrentSize > 0) CurrentSize--;
        }
    }
}
