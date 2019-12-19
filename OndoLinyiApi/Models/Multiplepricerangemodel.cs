using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OndoLinyiApi.Models
{
    public class Multiplepricerangemodel
    {
        public int Id { set; get; }
        public int Productid {set;get;}
        public int Lowerbound { set; get; }
        public int Upperbound { set; get; }
        public decimal Price { set; get; }
    }
}
