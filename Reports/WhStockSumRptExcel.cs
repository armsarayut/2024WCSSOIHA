using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using GoWMS.Server.Data;
using GoWMS.Server.Models.Inv;
namespace GoWMS.Server.Reports
{
    public class WhStockSumRptExcel
    {
        MemoryStream _memoryStream = new MemoryStream();
        //List<Vrpt_shelf_listInfo> ListReport = new List<Vrpt_shelf_listInfo>();
        public byte[] Report(List<InvStockSum> ListRpt)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("2.1");
                #region Excel Report Header
                var imagePath = VarGlobals.Imagelogoreport();
                worksheet.Column(1).Width = 24;
                worksheet.Row(1).Height = 30;
                var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")); //this will throw an error
                var mWidthlogo = VarGlobals.Widthlogoreport();
                var mHighlogo = VarGlobals.Highlogoreport();
                image.ScaleWidth(mWidthlogo);
                image.ScaleHeight(mHighlogo);
                worksheet.Cell("B1").Value = "2.1.Stock Summary" + " - Report";
                worksheet.Cell("B1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                worksheet.Cell("B2").Value = $"PrintDate : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
                #endregion Excel

                #region Excel Report Data
                var rptRows = 4;
                worksheet.Cell(rptRows, 1).Value = "ITEM";
                worksheet.Cell(rptRows, 2).Value = "NAME";
                worksheet.Cell(rptRows, 3).Value = "BATCH";
                worksheet.Cell(rptRows, 4).Value = "QTY";
 
                foreach (var rpt in ListRpt)
                {
                    rptRows++;
                    worksheet.Cell(rptRows, 1).Value = "'" + rpt.Item_code;
                    worksheet.Cell(rptRows, 2).Value = "'" + rpt.Item_name;
                    worksheet.Cell(rptRows, 3).Value = "'" + rpt.lot;
                    worksheet.Cell(rptRows, 4).Value = "'" + string.Format(VarGlobals.FormatN2, rpt.DisTotalstock); 
                }
                #endregion
                workbook.SaveAs(_memoryStream);
            }
            return _memoryStream.ToArray();
        }
    }
}
