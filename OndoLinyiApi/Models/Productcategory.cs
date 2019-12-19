using System;
using System.Collections.Generic;

namespace OndoLinyiApi.Models
{
    public partial class Productcategory
    {
        public int Catid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = "";
    }
}
