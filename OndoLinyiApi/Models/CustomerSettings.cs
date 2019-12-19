using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OndoLinyiApi.Models
{
    public class Customersettings
    {
        public Customersettings() { }

        public Customersettings(string customer)
        {
            this.Customer = customer;
        }
        public int Id { set; get; }
        public string Customer { set; get; }
        public byte Allowsms { set; get; } = 0;
        public byte Allowemail { set; get; } = 1;
        public byte Notifyorderstatus { set; get; } = 1;
    }
}
