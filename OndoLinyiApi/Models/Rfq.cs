using System;
using System.Collections.Generic;

namespace OndoLinyiApi.Models
{
    public partial class Rfq
    {
        public string Rfqid { get; set; }
        public string Customer { get; set; }
        public int Productid { get; set; }
        public string Specification { get; set; }
        public DateTime? Requestdate { get; set; } = DateTime.Now;
        public int Qty { get; set; }
        public string Location { get; set; }
        public string Address { set; get; }
        public string State { set; get; }
        public string Salesrep { set; get; }
    }
}
