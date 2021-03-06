namespace Neonatology.Areas.Administration.Services
{
    using ViewModels.Administration.Galery;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGalleryService
    {
        Task<ICollection<GaleryViewModel>> GetGaleryImages();

        Task<bool> Delete(string id);
    }
}
