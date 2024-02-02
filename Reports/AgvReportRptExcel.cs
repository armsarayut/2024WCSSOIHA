using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using GoWMS.Server.Data;
using GoWMS.Server.Models.Wcs;
using GoWMS.Server.Models.Hagv;

namespace GoWMS.Server.Reports
{
    public class AgvReportRptExcel
    {
        MemoryStream _memoryStream = new MemoryStream();
        List<Vrptqueueloadtimeagv> _ListRpt = new List<Vrptqueueloadtimeagv>();
        public byte[] Report(List<Vrptqueueloadtimeagv> ListRpt)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("5.5.6");
                int startRows = 4;
                #region Excel Report Header
                var imagePath = VarGlobals.Imagelogoreport();
                worksheet.Column(1).Width = 18;
                worksheet.Row(1).Height = 60;
                var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")); //this will throw an error
                image.ScaleWidth(.35);
                image.ScaleHeight(.2);
                worksheet.Cell("B1").Value = "5.5.8.AGV" + " Report";
                worksheet.Cell("B1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                worksheet.Cell("B2").Value = $"PrintDate : {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
                #endregion Excel

                #region Excel Report Data
                var rptRows = 4;
                worksheet.Cell(rptRows, 1).Value = "CREATED";
                worksheet.Cell(rptRows, 2).Value = "PALLET";
                worksheet.Cell(rptRows, 3).Value = "AGV";
                worksheet.Cell(rptRows, 4).Value = "FROM";
                worksheet.Cell(rptRows, 5).Value = "TO";
                worksheet.Cell(rptRows, 6).Value = "STARTED";
                worksheet.Cell(rptRows, 7).Value = "COMPLETED";
                worksheet.Cell(rptRows, 8).Value = "TIME";

                foreach (var rpt in ListRpt)
                {
                    rptRows++;
                    worksheet.Cell(rptRows, 1).Value = Convert.ToDateTime(rpt.Ctime).ToString(VarGlobals.FormatDT);
                    worksheet.Cell(rptRows, 2).Value = rpt.Lpncode;
                    worksheet.Cell(rptRows, 3).Value = rpt.Agv_name;
                    worksheet.Cell(rptRows, 4).Value = rpt.Gate_source;
                    worksheet.Cell(rptRows, 5).Value = rpt.Gate_dest;
                    worksheet.Cell(rptRows, 6).Value = Convert.ToDateTime(rpt.Stime).ToString(VarGlobals.FormatDT);
                    worksheet.Cell(rptRows, 7).Value = Convert.ToDateTime(rpt.Etime).ToString(VarGlobals.FormatDT);
                    worksheet.Cell(rptRows, 8).Value = rpt.Loadtime;
                }
                #endregion

                worksheet.SheetView.Freeze(startRows, 1);
                workbook.SaveAs(_memoryStream);
            }
            return _memoryStream.ToArray();
        }
    }
}
