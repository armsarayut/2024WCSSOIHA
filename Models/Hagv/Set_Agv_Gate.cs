using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoWMS.Server.Models.Hagv
{
    public class Set_Agv_Gate
    {
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public Int64? Client_Id { get; set; }
        public string Client_Ip { get; set; }
        public string Gate_Name { get; set; }
        public string Position_Code { get; set; }
        public string Area { get; set; }
        public Int32? Gate_type { get; set; }
    }
}
