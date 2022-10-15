namespace Neonatology.Web.Areas.Administration.Controllers;

using System;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using ViewModels.Administration.Qr;
using static QRCoder.PayloadGenerator;

public class QrController : BaseController
{
    [HttpGet]
    public IActionResult CreateQrCode() 
        => this.View();

    [HttpPost]
    public IActionResult CreateQrCode(QRCodeModel qRCode)
    {
        var webUri = new Url(qRCode.QRCodeText);
        var uriPayload = webUri.ToString();

        var qrGenerator = new QRCodeGenerator();
        var qrCodeInfo = qrGenerator.CreateQrCode(uriPayload, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrCodeInfo);

        var bitmapArray = qrCode.GetGraphic(60);
        var qrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(bitmapArray));
        this.ViewBag.QrCodeUri = qrUri;

        return this.View();
    }
}