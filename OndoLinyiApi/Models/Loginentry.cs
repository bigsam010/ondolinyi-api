using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OndoLinyiApi.Models
{
    public class Loginentry
    {
        public int Id { set; get; }
        public string Client { set; get; }
        public DateTime Logindate { set; get; } = DateTime.Now;

    }
}
