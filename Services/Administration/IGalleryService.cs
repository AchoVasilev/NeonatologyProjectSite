namespace Services.Administration
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ViewModels.Administration.Galery;

    public interface IGalleryService
    {
        Task<ICollection<GaleryViewModel>> GetGaleryImages();

        Task<bool> Delete(string id);
    }
}
