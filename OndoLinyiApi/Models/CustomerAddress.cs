using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OndoLinyiApi.Models
{
    public class CustomerAddress
    {
        public int Id { set; get; }
        public string Customer { set; get; }
        public string Address1 { set; get; }
        public string Address2 { set; get; }
        public string City { set; get; }
        public string State { set; get; }
        public string Country { set; get; }
        public string Phone1 { set; get; }
        public string Phone2 { set; get; }
        public byte Isdefault { set; get; }

    }
}
