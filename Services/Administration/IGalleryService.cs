namespace Services.Administration
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ViewModels.Administration.Galery;

    public interface IGalleryService
    {
        Task<ICollection<GalleryViewModel>> GetGalleryImages();

        Task<bool> Delete(string id);
    }
}
