using System;
using System.Collections.Generic;

namespace OndoLinyiApi.Models
{
    public partial class Product
    {
        public int Productid { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Pricemodel { get; set; }
        public int Manufacturer { get; set; }
        public int Moq { get; set; }
        public int Taxtype { get; set; }
        public string Measureunit { get; set; }
        public int? Qtyavailable { get; set; }
        public int? Surveywaitingdays { get; set; }
        public DateTime? Dateadded { get; set; } = DateTime.Now;
        public string Status { get; set; }
        public string Pagetitle { get; set; }
        public int Sku { get; set; }
        public string Mappedurl { get; set; }
        public string Seodescription { get; set; }
    }
}
