namespace ViewModels.Administration.Galery
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using Microsoft.AspNetCore.Http;

    public class GalleryModel
    {
        private const string ImagesName = "Качи снимки";

        [DisplayName(ImagesName)]
        public IEnumerable<IFormFile> Images { get; set; }

        public ICollection<GalleryViewModel> GalleryImages { get; set; }
    }
}
