namespace Neonatology.Areas.Administration.Services
{
    using Neonatology.Areas.Administration.ViewModels.Galery;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGaleryService
    {
        Task<ICollection<GaleryViewModel>> GetGaleryImages();
    }
}
