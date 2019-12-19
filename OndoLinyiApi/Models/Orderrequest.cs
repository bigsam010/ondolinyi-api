using System;
using System.Collections.Generic;

namespace OndoLinyiApi.Models
{
    public partial class Orderrequest
    {
        public string Orderid { get; set; }
        public string Rfqid { get; set; }
        public DateTime? Requestdate { get; set; }
        public string Status { get; set; }
        public DateTime Deliverydate { get; set; }
        public byte Reviewed { get; set; }
        public int Productid { get; set; }
    }
}
