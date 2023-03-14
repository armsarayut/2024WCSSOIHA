using iTextSharp.text;
using iTextSharp.text.pdf;
using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoWMS.Server.Models;
using GoWMS.Server.Data;
using System.Drawing;

namespace GoWMS.Server.Reports
{
    public class KimballPallet4x1_5 : PdfPageEvents
    {
        readonly BaseFont mpdfFont = BaseFont.CreateFont(VarGlobals.Fontreport(), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

        #region GeneratePDF
        public byte[] ExportPDF(List<Pallet4x1_5> ListRpts)
        {
            BaseFont baseFont = mpdfFont;
            iTextSharp.text.Font fonheader = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD);

            ////                    Set paper                        (4" , 2") Note 1" = 2.54 cm = 72
            ///Document doc = new Document(new iTextSharp.text.Rectangle(288, 144), 5, 5, 1, 1);
            Document doc = new Document(new iTextSharp.text.Rectangle(288, 108), 5, 5, 1, 1);
            MemoryStream ms = new MemoryStream();
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();
                int i = 0;

                foreach (var listRpt in ListRpts)
                {
                    if (i != 0)
                        doc.NewPage();

                   
                    iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                    //iTextSharp.text.pdf.BarcodeQRCode qrcodes = new BarcodeQRCode("testing", 50, 50, null);
                    //string strBarCodeValue = "hello world";
                    //BarcodeQRCode barcodeQRCode = new BarcodeQRCode(strBarCodeValue, 20, 20, null);


                    //QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    //QRCodeData qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", QRCodeGenerator.ECCLevel.Q);
                    //QRCode qrCode = new QRCode(qrCodeData);
                    //Bitmap qrCodeImage = qrCode.GetGraphic(20);



                    //iTextSharp.text.pdf.Barcode128 bc = new Barcode128();
                    //bc.TextAlignment = Element.ALIGN_CENTER;
                    //bc.Code = listRpt.Palletcode.ToString();
                    //bc.StartStopText = false;
                    //bc.CodeType = iTextSharp.text.pdf.Barcode128.CODE128;
                    //bc.Extended = true;
                    //bc.Font = null;
                    //iTextSharp.text.Image img = bc.CreateImageWithBarcode(cb, iTextSharp.text.BaseColor.Black, iTextSharp.text.BaseColor.Black);


                    ////qrCodeImage.Save("Qrcoder.bmp");

                    //QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
                    //QRCodeData qrCodeData = qrGenerator.CreateQrCode(listRpt.Palletcode.ToString(), QRCodeGenerator.ECCLevel.Q);
                    //QRCode qrcode = new QRCode(qrCodeData);

                    //// GetGraphic 第一个参数设置图形的大小
                    //Bitmap qrCodeImage = qrcode.GetGraphic(3, Color.Black, Color.White, null, 15, 1, false);


                    //MemoryStream ms = new MemoryStream();
                    //bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);


                    //QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    //QRCodeData qrCodeData = qrGenerator.CreateQrCode(listRpt.Palletcode.ToString(), QRCodeGenerator.ECCLevel.L);
                    //QRCode qrCode = new QRCode(qrCodeData);
                    //Bitmap bitmap = qrCode.GetGraphic(20);
                    ////bitmap.Save("qrcode.bmp");
                    //MemoryStream msqr = new MemoryStream();
                    //bitmap.Save(msqr, System.Drawing.Imaging.ImageFormat.Png);
                    //Byte[] qrcodeByte = msqr.ToArray();



                    QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                    QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(listRpt.Palletcode.ToString(), QRCodeGenerator.ECCLevel.Q);
                    PngByteQRCode pngByteQRCode = new PngByteQRCode(qRCodeData);
                    byte[] qrCodeBytes = pngByteQRCode.GetGraphic(20);


               

                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(qrCodeBytes);



                    cb.SetTextMatrix(70.0f, 253.0f);
                    img.ScaleToFit(80, 288);
                    img.SetAbsolutePosition(108f, 30f);
                    img.Alignment = Element.ALIGN_CENTER;
                    cb.AddImage(img);

                    PdfContentByte cb13 = writer.DirectContent;
                    cb13.BeginText();
                    cb13.SetFontAndSize(baseFont, 18.0f);
                    cb13.ShowTextAligned(Element.ALIGN_CENTER, listRpt.Palletcode.ToString(), 150f, 20f, 0);
                    cb13.EndText();


                    i++;
                }
            }
            catch
            {
            }
            finally
            {
                doc.Close();
            }
            byte[] buff = ms.ToArray();
            return buff;

            #endregion
        }
    }
}
