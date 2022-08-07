namespace Services.Administration;

using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using global::Common.Models;
using ViewModels.Administration.Galery;

public interface IGalleryService : ITransientService
{
    Task<ICollection<GalleryViewModel>> GetGalleryImages();

    Task<OperationResult> Delete(string id);
}