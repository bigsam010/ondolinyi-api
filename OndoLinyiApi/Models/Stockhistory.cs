using System;
using System.Collections.Generic;

namespace OndoLinyiApi.Models
{
    public partial class Stockhistory
    {
        public int Logid { get; set; }
        public int Productid { get; set; }
        public int? Qtyavailable { get; set; }
        public int Qtyadded { get; set; }
        public string Addedby { get; set; }
        public DateTime? Dateadded { get; set; }
    }
}
