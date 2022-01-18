namespace Neonatology.Areas.Administration.ViewModels.Galery
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using Microsoft.AspNetCore.Http;

    public class GaleryModel
    {
        private const string ImagesName = "Качи снимки";

        [DisplayName(ImagesName)]
        public IEnumerable<IFormFile> Images { get; set; }

        public ICollection<GaleryViewModel> GaleryImages { get; set; }
    }
}
