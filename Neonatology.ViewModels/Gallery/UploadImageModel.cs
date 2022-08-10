namespace Neonatology.ViewModels.Gallery;

using System.Collections.Generic;
using System.ComponentModel;
using Common.Mapping;
using Data.Models;
using Microsoft.AspNetCore.Http;

public class UploadImageModel : IMapFrom<Image>
{
    private const string ImagesName = "Снимки";

    [DisplayName(ImagesName)]
    public IEnumerable<IFormFile> Images { get; set; }
}