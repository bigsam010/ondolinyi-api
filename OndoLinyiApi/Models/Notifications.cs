using System;
using System.Collections.Generic;

namespace OndoLinyiApi.Models
{
    public partial class Notifications
    {
        public int Notifyid { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public byte Viewed { get; set; }
        public DateTime? Notifydate { get; set; }
        public string Customer { get; set; }
    }
}
