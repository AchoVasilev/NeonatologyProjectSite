namespace Neonatology.Web.Areas.Administration.Controllers;

using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System;

using Microsoft.AspNetCore.Mvc;
using ViewModels.Administration.Qr;
using static QRCoder.PayloadGenerator;
using QRCoder;

public class QrController : BaseController
{
    [HttpGet]
    public IActionResult CreateQRCode() 
        => this.View();

    [HttpPost]
    public IActionResult CreateQRCode(QRCodeModel qRCode)
    {
        var webUri = new Url(qRCode.QRCodeText);
        var uriPayload = webUri.ToString();

        var qrGenerator = new QRCodeGenerator();
        var qrCodeInfo = qrGenerator.CreateQrCode(uriPayload, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeInfo);

        var BitmapArray = qrCode.GetGraphic(60);
        var QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
        this.ViewBag.QrCodeUri = QrUri;

        return this.View();
    }
}

//Extension method to convert Bitmap to Byte Array
public static class BitmapExtension
{
    public static byte[] BitmapToByteArray(this Bitmap bitmap)
    {
        using (var ms = new MemoryStream())
        {
            bitmap.Save(ms, ImageFormat.Png);

            return ms.ToArray();
        }
    }
}