using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoWMS.Server.Models.Public
{
    public class Set_Workstation
    {
         
        public Int64? Idx { get; set; }
        public DateTime? Created { get; set; }
        public Int32? Entity_Lock { get; set; }
        public DateTime? Modified { get; set; }
        public Int64? Client_Id { get; set; }
        public string Client_Ip { get; set; }
        public string Stcode { get; set; }
        public string Stdesc { get; set; }
        public string Stref { get; set; }
        
    }
}
