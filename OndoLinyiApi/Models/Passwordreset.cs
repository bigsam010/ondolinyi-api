using System;
using System.Collections.Generic;

namespace OndoLinyiApi.Models
{
    public partial class Passwordreset
    {
        public int Reqid { get; set; }
        public string Token { get; set; }
        public string Customer { get; set; }
        public DateTime? Reqdate { get; set; } = DateTime.Now;
        public DateTime? Expdate { get; set; }
    }
}
