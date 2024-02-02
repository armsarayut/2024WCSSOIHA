﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using GoWMS.Server.Data;
using GoWMS.Server.Models.Public;

namespace GoWMS.Server.Reports
{
    public class PaM62ARptExcel
    {
        MemoryStream _memoryStream = new MemoryStream();
        //List<Inb_Goodreceipt_Go> _Inb_Goodreceive_Go_s = new List<Inb_Goodreceipt_Go>();
        public byte[] Report(List<Class6_2_A> rptElements)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("5.2.1");
                #region Excel Report Header
                var imagePath = VarGlobals.Imagelogoreport();
                worksheet.Column(1).Width = 24;
                worksheet.Row(1).Height = 30;
                var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")); //this will throw an error
                var mWidthlogo = VarGlobals.Widthlogoreport();
                var mHighlogo = VarGlobals.Highlogoreport();
                image.ScaleWidth(mWidthlogo);
                image.ScaleHeight(mHighlogo);
                worksheet.Cell("B1").Value = "5.2.1.Order Receive" + " - Report";
                worksheet.Cell("B1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                worksheet.Cell("B2").Value = $"PrintDate : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
                #endregion Excel

                #region Excel Report Data
                var rptRows = 4;
                worksheet.Cell(rptRows, 1).Value = "DATE";
                worksheet.Cell(rptRows, 2).Value = "TASKNO";
                worksheet.Cell(rptRows, 3).Value = "ITEM";
                worksheet.Cell(rptRows, 4).Value = "NAME";
                worksheet.Cell(rptRows, 5).Value = "BATCH";
                worksheet.Cell(rptRows, 6).Value = "QTY";
                worksheet.Cell(rptRows, 7).Value = "PALLET";

                foreach (var rpt in rptElements)
                {
                    rptRows++;
                    worksheet.Cell(rptRows, 1).Value = "'" + Convert.ToDateTime(rpt.Created).ToString(VarGlobals.FormatDT) ;
                    worksheet.Cell(rptRows, 2).Value = "'" + rpt.Po_No;
                    worksheet.Cell(rptRows, 3).Value = "'" + rpt.Item_Code;
                    worksheet.Cell(rptRows, 4).Value = "'" + rpt.Item_Name;
                    worksheet.Cell(rptRows, 5).Value = "'" + rpt.Batch_Number;
                    worksheet.Cell(rptRows, 6).Value = "'" + string.Format(VarGlobals.FormatN2, rpt.DisResult_Qty);
                    worksheet.Cell(rptRows, 7).Value = "'" + rpt.Pallet_No;
                }
                #endregion
                workbook.SaveAs(_memoryStream);
            }
            return _memoryStream.ToArray();
        }

    }
}
