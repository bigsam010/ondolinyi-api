using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OndoLinyiApi.Models
{
    public class Pricerangemodel
    {
        public int Productid { set; get; }
        public decimal Lowprice { set; get; }
        public decimal Highprice { set; get; }
    }
}
