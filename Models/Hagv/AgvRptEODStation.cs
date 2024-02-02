using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoWMS.Server.Models.Hagv
{
    public class AgvRptEODStation
    {
        public DateTime? W_date { get; set; }
        public string Gate_source { get; set; }
        public string Gate_dest { get; set; }
        public long? W_count { get; set; }
    }
}
