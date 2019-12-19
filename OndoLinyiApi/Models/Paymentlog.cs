using System;
using System.Collections.Generic;

namespace OndoLinyiApi.Models
{
    public partial class Paymentlog
    {
        public string Refno { get; set; }
        public string Card { get; set; }
        public string Invoicenumber { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime? Paymentdate { get; set; }
        public string Mode { get; set; }
        public decimal Totalamount { get; set; }
        public string Rfqid { get; set; }
    }
}
