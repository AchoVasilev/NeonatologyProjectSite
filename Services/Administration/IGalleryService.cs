namespace Services.Administration;

using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using ViewModels.Administration.Galery;

public interface IGalleryService
{
    Task<ICollection<GalleryViewModel>> GetGalleryImages();

    Task<OperationResult> Delete(string id);
}