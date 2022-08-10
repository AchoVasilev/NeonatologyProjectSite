
namespace Test.AdministrationArea.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using Mocks;
using Neonatology.Services.Administration;
using Neonatology.ViewModels.Administration.Gallery;
using Xunit;

public class GalleryServiceTests
{
    [Fact]
    public async Task GetGaleryImagesShouldReturnListOfGaleryViewModel()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var service = new GalleryService(dataMock, mapperMock, null, null);
        var result = await service.GetGalleryImages();

        Assert.IsAssignableFrom<List<GalleryViewModel>>(result);
    }
}