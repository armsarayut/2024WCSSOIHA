using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoWMS.Server.Models.Oub
{
    public class Inv_Stock_GoApiInfo
    {
        public Int64? Efidx { get; set; }
        public Int32? Efstatus { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public Int64? Innovator { get; set; }
        public string Device { get; set; }
        public string Pono { get; set; }
        public string Pallettag { get; set; }
        public string Itemtag { get; set; }
        public string Itemcode { get; set; }
        public string Itemname { get; set; }
        public string Itembar { get; set; }
        public string Unit { get; set; }
        public string Weightunit { get; set; }
        public Decimal? Quantity { get; set; }
        public Decimal? Weight { get; set; }
        public string Lotno { get; set; }
        public Decimal? Totalquantity { get; set; }
        public Decimal? Totalweight { get; set; }
        public string Docno { get; set; }
        public string Docby { get; set; }
        public DateTime? Docdate { get; set; }
        public string Docnote { get; set; }
        public Int64? Grnrefer { get; set; }
        public DateTime? Grntime { get; set; }
        public DateTime? Grtime { get; set; }
        public string Grtype { get; set; }
        public string Pallteno { get; set; }
        public string Palltmapkey { get; set; }
        public DateTime? Storagetime { get; set; }
        public string Storageno { get; set; }
        public string Storagearea { get; set; }
        public string Storagebin { get; set; }
        public Int64? Gnrefer { get; set; }
        public Decimal? Allocatequantity { get; set; }
        public Decimal? Allocateweight { get; set; }

        public Int64? Apiefidx { get; set; }
        public Int32? Apiefstatus { get; set; }
        public DateTime? Apicreated { get; set; }
        public DateTime? Apimodified { get; set; }
        public Int64? Apiinnovator { get; set; }
        public string Apidevice { get; set; }

        public string ApipackageID { get; set; }
        public string ApirollID { get; set; }
        public string ApimaterialCode { get; set; }
        public string ApimaterialDescription { get; set; }
       public DateTime? ApireceivingDate { get; set; }
        public decimal? ApigrQuantity { get; set; }
        public string ApiUnit { get; set; }
        public decimal? apigrQuantityKG { get; set; }
        public string ApiwhCode { get; set; }
        public string Apiwarehouse { get; set; }
        public string Apijob { get; set; }

        public string ApijobCode { get; set; }
        public string Apitypor { get; set; }
        public string Apikaror { get; set; }

    }
}
