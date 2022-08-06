namespace ViewModels.Administration.Qr;

using System.ComponentModel.DataAnnotations;

public class QRCodeModel
{

    [Display(Name = "Добави QR текст")]
    public string QRCodeText { get; set; }
}