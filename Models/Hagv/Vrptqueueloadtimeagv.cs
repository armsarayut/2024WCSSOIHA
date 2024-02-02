using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoWMS.Server.Models.Hagv
{
    public class Vrptqueueloadtimeagv
    {
        public string Lpncode { get; set; }
        public string Work_code { get; set; }
        public Int32? Work_status { get; set; }
        public string Agv_name { get; set; }
        public string Gate_source { get; set; }
        public string Gate_dest { get; set; }
        public DateTime? Ctime { get; set; }
        public DateTime? Stime { get; set; }
        public DateTime? Etime { get; set; }
        public string Loadtime { get; set; }
        public string Work_mode { get; set; }
    }
}
