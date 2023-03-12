using System;

namespace GoWMS.Server.Models.Yss
{
    public class Posttaskorders
    {
        public Int64? Efidx { get; set; }
        public Int32? Efstatus { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public Int64? Innovator { get; set; }
        public string Device { get; set; }
        public string Taskno { get; set; }
        public string Tasktype { get; set; }
        public string Palletcode { get; set; }
        public string Itemno { get; set; }
        public string Batchno { get; set; }
        public decimal? Qty { get; set; }

        public DateTime? Senddate { get; set; }
        public string Sendby { get; set; }
        public string Pickgate { get; set; }


    }
}
