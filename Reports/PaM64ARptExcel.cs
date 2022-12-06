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
    public class PaM64ARptExcel
    {
        MemoryStream _memoryStream = new MemoryStream();
        //List<Inb_Goodreceipt_Go> _Inb_Goodreceive_Go_s = new List<Inb_Goodreceipt_Go>();
        public byte[] Report(List<Class6_4_A> rptElements)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("5.4.1");
                #region Excel Report Header
                var imagePath = VarGlobals.Imagelogoreport();
                worksheet.Column(1).Width = 24;
                worksheet.Row(1).Height = 30;
                var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")); //this will throw an error
                image.ScaleWidth(.18);
                image.ScaleHeight(.18);
                worksheet.Cell("B1").Value = "5.4.1.Order picking" + " - Report";
                worksheet.Cell("B1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                worksheet.Cell("B2").Value = $"PrintDate : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
                #endregion Excel

                #region Excel Report Data
                var rptRows = 4;
                worksheet.Cell(rptRows, 1).Value = "DATE";
                worksheet.Cell(rptRows, 2).Value = "JOB";
                worksheet.Cell(rptRows, 3).Value = "PART";
                worksheet.Cell(rptRows, 4).Value = "NAME";
                worksheet.Cell(rptRows, 5).Value = "SU";
                worksheet.Cell(rptRows, 6).Value = "BATCH";
                worksheet.Cell(rptRows, 7).Value = "QTY";

                foreach (var rpt in rptElements)
                {
                    rptRows++;
                    worksheet.Cell(rptRows, 1).Value = "'" + Convert.ToDateTime(rpt.Created).ToString(VarGlobals.FormatD);
                    worksheet.Cell(rptRows, 2).Value = "'" + rpt.Order_No;
                    worksheet.Cell(rptRows, 3).Value = "'" + rpt.Item_Code;
                    worksheet.Cell(rptRows, 4).Value = "'" + rpt.Item_Name;
                    worksheet.Cell(rptRows, 5).Value = "'" + rpt.Su_No;
                    worksheet.Cell(rptRows, 6).Value = "'" + rpt.Batch_Number;
                    worksheet.Cell(rptRows, 7).Value = "'" + string.Format(VarGlobals.FormatN3, rpt.DisResult_Qty);


                }
                #endregion
                workbook.SaveAs(_memoryStream);
            }
            return _memoryStream.ToArray();
        }
    }
}
