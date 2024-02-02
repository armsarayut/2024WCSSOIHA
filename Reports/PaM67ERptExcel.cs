using System;
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
    public class PaM67ERptExcel
    {
        MemoryStream _memoryStream = new MemoryStream();
        //List<Inb_Goodreceipt_Go> _Inb_Goodreceive_Go_s = new List<Inb_Goodreceipt_Go>();
        public byte[] Report(List<Class6_7_F> rptElements)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("5.5.5");
                #region Excel Report Header
                var imagePath = VarGlobals.Imagelogoreport();
                worksheet.Column(1).Width = 24;
                worksheet.Row(1).Height = 30;
                var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")); //this will throw an error
                var mWidthlogo = VarGlobals.Widthlogoreport();
                var mHighlogo = VarGlobals.Highlogoreport();
                image.ScaleWidth(mWidthlogo);
                image.ScaleHeight(mHighlogo);
                worksheet.Cell("B1").Value = "5.5.5.ASRS-End of day" + " - Report";
                worksheet.Cell("B1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                worksheet.Cell("B2").Value = $"PrintDate : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
                #endregion Excel

                #region Excel Report Data
                var rptRows = 4;

                worksheet.Cell(rptRows, 1).Value = "DATE";
                worksheet.Cell(rptRows, 2).Value = "TOTAL";
                worksheet.Cell(rptRows, 3).Value = "STORE-IN";
                worksheet.Cell(rptRows, 4).Value = "STORE-OUT";
                worksheet.Cell(rptRows, 5).Value = "EMPTY-IN";
                worksheet.Cell(rptRows, 6).Value = "EMPTY-OUT";
                worksheet.Cell(rptRows, 5).Value = "MOVE";

                foreach (var rpt in rptElements)
                {
                    rptRows++;
                    worksheet.Cell(rptRows, 1).Value = "'" + Convert.ToDateTime(rpt.W_date).ToString(VarGlobals.FormatD);
                    worksheet.Cell(rptRows, 2).Value = "'" + string.Format(VarGlobals.FormatN0, rpt.Wtotal);
                    worksheet.Cell(rptRows, 3).Value = "'" + string.Format(VarGlobals.FormatN0, rpt.W01);
                    worksheet.Cell(rptRows, 4).Value = "'" + string.Format(VarGlobals.FormatN0, rpt.W05);
                    worksheet.Cell(rptRows, 5).Value = "'" + string.Format(VarGlobals.FormatN0, rpt.W101);
                    worksheet.Cell(rptRows, 6).Value = "'" + string.Format(VarGlobals.FormatN0, rpt.W102);
                    worksheet.Cell(rptRows, 5).Value = "'" + string.Format(VarGlobals.FormatN0, rpt.W09);
                }
                #endregion
                workbook.SaveAs(_memoryStream);
            }
            return _memoryStream.ToArray();
        }
    }
}
