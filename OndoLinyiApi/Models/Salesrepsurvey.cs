using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OndoLinyiApi.Models
{
    public class Salesrepsurvey
    {
        public int Surveyid { get; set; }
        public string SalesRep { get; set; }
        public string Surveyby { get; set; }
        public int Rating { get; set; }
        public string Remark { get; set; }
        public DateTime? Surveydate { get; set; }
    }
}
