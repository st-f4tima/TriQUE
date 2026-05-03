using System;
using Trique.Forms;
using TriQue.Enums;
using TriQue.Forms;

namespace TriQue.Models
{
    public class Admin : User
    {
        public int AdminID { get; set; }
        public AdminLevel Level { get; set; }
        public override Form GetView()
        {
            return new AdminForm(UserID);
        }

    }
}
