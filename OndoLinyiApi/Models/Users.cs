using System;
using System.Collections.Generic;

namespace OndoLinyiApi.Models
{
    public partial class Users
    {
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Role { get; set; }
        public DateTime Joindate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Active";
        public string Phone { get; set; }
        public string Bio { get; set; }
        public string Password { get; set; }
    }
}
