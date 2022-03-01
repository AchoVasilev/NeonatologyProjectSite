namespace ViewModels.Galery
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using Common;

    using Microsoft.AspNetCore.Http;

    public class UploadImageModel : PagingModel
    {
        private const string ImagesName = "Снимки";

        [DisplayName(ImagesName)]
        public IEnumerable<IFormFile> Images { get; set; }

        public List<string> Urls { get; init; } = new List<string>();
    }
}
