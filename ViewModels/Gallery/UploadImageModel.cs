namespace ViewModels.Gallery;

using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.AspNetCore.Http;

public class UploadImageModel
{
    private const string ImagesName = "Снимки";

    [DisplayName(ImagesName)]
    public IEnumerable<IFormFile> Images { get; set; }
}