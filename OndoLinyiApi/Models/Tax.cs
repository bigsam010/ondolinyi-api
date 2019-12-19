using System;
using System.Collections.Generic;

namespace OndoLinyiApi.Models
{
    public partial class Tax
    {
        public int Taxid { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Value { get; set; }
        public string Invoicelabel { get; set; }
        public string Description { get; set; }
    }
}
