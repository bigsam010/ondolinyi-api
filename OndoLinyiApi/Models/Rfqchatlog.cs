using System;
using System.Collections.Generic;

namespace OndoLinyiApi.Models
{
    public partial class Rfqchatlog
    {
        public int Chatid { get; set; }
        public string Salesrep { get; set; }
        public string Rfqid { get; set; }
        public byte Viewed { get; set; }
        public string Status { get; set; }
        public DateTime? Chaatdate { get; set; }
    }
}
