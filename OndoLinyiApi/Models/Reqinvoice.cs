using System;
using System.Collections.Generic;

namespace OndoLinyiApi.Models
{
    public partial class Reqinvoice
    {
        public string Invoicenubmer { get; set; }
        public decimal? Taxamount { get; set; }
        public decimal? Deliverycost { get; set; }
        public string Rfqid { get; set; }
        public decimal Productprice { get; set; }
        public string Status { get; set; }
        public decimal Totalamount { get; set; }
        public int Quantity { get; set; }
    }
}
