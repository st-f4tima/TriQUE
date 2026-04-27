using System;

namespace TriQue.Models
{
    public class AuthenticationLog
    {
        public int LogID { get; set; }
        public int UserID { get; set; }
        public DateTime LoginTime { get; set; } = DateTime.Now;
        public DateTime LogoutTime { get; set; }
        public string AuthOutcome { get; set; } = string.Empty;
    }
}
