namespace ViewModels.Administration.Qr
{
    using System;
    using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
using System.Xml.Linq;

    public class QRCodeModel
    {

        [Display(Name = "Добави QR текст")]
        public string QRCodeText { get; set; }
    }
}
