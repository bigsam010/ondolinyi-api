using System;
using System.Collections.Generic;

namespace OndoLinyiApi.Models
{
    public partial class Manufacturer
    {
        public int Manufacturerid { get; set; }
        public string Companyname { get; set; }
        public string Companyaddress { get; set; }
        public string Aboutcompany { get; set; }
        public string Companyemail { get; set; }
        public string Companyphone { get; set; }
        public string Status { get; set; } = "Active";
        public string City { get; set; }
        public string Rcno { get; set; }
        public string Legalname { get; set; }
    }
}
