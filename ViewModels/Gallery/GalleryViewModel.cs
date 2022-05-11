namespace ViewModels.Gallery
{
    using System.Collections.Generic;

    using Common;

    public class GalleryViewModel : PagingModel
    {
        public ICollection<string> ImageUrls { get; set; } = new List<string>();
    }
}
