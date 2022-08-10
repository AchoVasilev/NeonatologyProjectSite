namespace Neonatology.Services.Administration;

using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Neonatology.Common.Models;
using ViewModels.Administration.Gallery;

public interface IGalleryService : ITransientService
{
    Task<ICollection<GalleryViewModel>> GetGalleryImages();

    Task<OperationResult> Delete(string id);
}