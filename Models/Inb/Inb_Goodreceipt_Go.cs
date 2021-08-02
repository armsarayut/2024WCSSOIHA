﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoWMS.Server.Models.Inb
{
	public class Inb_Goodreceipt_Go
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
		public string Grntype { get; set; }

	}
}
