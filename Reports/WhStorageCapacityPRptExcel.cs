using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using GoWMS.Server.Data;
using GoWMS.Server.Models;

namespace GoWMS.Server.Reports
{
    public class WhStorageCapacityPRptExcel
    {
        MemoryStream _memoryStream = new MemoryStream();
        //List<Vrpt_shelf_listInfo> ListReport = new List<Vrpt_shelf_listInfo>();
        public byte[] Report(List<WhStorageCapacity> ListRpt)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("2.2");
                #region Excel Report Header
                var imagePath = VarGlobals.Imagelogoreport();
                worksheet.Column(1).Width = 24;
                worksheet.Row(1).Height = 30;
                var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")); //this will throw an error
                image.ScaleWidth(.13);
                image.ScaleHeight(.07);
                worksheet.Cell("B1").Value = "2.2.Capacity" + " - Report";
                worksheet.Cell("B1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                worksheet.Cell("B2").Value = $"PrintDate : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
                #endregion Excel

                #region Excel Report Data
                var rptRows = 4;
                worksheet.Cell(rptRows, 1).Value = "LANE";
                worksheet.Cell(rptRows, 2).Value = "USE";
                worksheet.Cell(rptRows, 3).Value = "FREE";
                worksheet.Cell(rptRows, 4).Value = "BLOCK/ERROR";
                worksheet.Cell(rptRows, 5).Value = "ProhibitedLocation";
                worksheet.Cell(rptRows, 6).Value = "TOTAL";
                worksheet.Cell(rptRows, 7).Value = "% USE";

                foreach (var rpt in ListRpt)
                {
                    rptRows++;
                    worksheet.Cell(rptRows, 1).Value = "'" + rpt.Srmname;
                    worksheet.Cell(rptRows, 2).Value = "'" + string.Format(VarGlobals.FormatN0, rpt.Locavlt1);  
                    worksheet.Cell(rptRows, 3).Value = "'" + string.Format(VarGlobals.FormatN0, rpt.Locemp);
                    worksheet.Cell(rptRows, 4).Value = "'" + string.Format(VarGlobals.FormatN0, rpt.Perr);
                    worksheet.Cell(rptRows, 5).Value = "'" + string.Format(VarGlobals.FormatN0, rpt.Prohloc);
                    worksheet.Cell(rptRows, 6).Value = "'" + string.Format(VarGlobals.FormatN0, rpt.Total);
                    worksheet.Cell(rptRows, 7).Value = "'" + string.Format(VarGlobals.FormatN0, rpt.OccRate);


                }
                #endregion
                workbook.SaveAs(_memoryStream);
            }
            return _memoryStream.ToArray();
        }
    }
}
