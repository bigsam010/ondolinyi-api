using System;
using System.Collections.Generic;

namespace OndoLinyiApi.Models
{
    public partial class Customercard
    {
        public string Cardnumber { get; set; }
        public string Customer { get; set; }
        public string Cvv { get; set; }
        public string Cardholder { get; set; }
        public string Expdate { get; set; }
    }
}
