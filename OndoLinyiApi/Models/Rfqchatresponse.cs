using System;
using System.Collections.Generic;

namespace OndoLinyiApi.Models
{
    public partial class Rfqchatresponse
    {
        public int Chatresid { get; set; }
        public int? Chatid { get; set; }
        public string Reponse { get; set; }
        public string Resby { get; set; }
        public string Type { get; set; }
        public byte Viewed { get; set; }
        public DateTime? Resdate { get; set; }
    }
}
