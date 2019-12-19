using System;
using System.Collections.Generic;

namespace OndoLinyiApi.Models
{
    public partial class Survey
    {
        public int Surveyid { get; set; }
        public string Orderid { get; set; }
        public string Surveyby { get; set; }
        public decimal Rating { get; set; }
        public int Remark { get; set; }
        public DateTime? Surveydate { get; set; }
        public string Status { get; set; }
    }
}
