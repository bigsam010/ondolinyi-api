using System;
using System.Collections.Generic;

namespace OndoLinyiApi.Models
{
    public partial class Customer
    {
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public string Verificationtoken { get; set; }
        public byte Isverified { get; set; } = 0;
        public DateTime Joindate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Active";
    }
}
