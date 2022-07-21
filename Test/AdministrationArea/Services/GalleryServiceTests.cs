
namespace Test.AdministrationArea.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Neonatology.Areas.Administration.Services;
    using Mocks;
    using ViewModels.Administration.Galery;
    using Xunit;

    public class GalleryServiceTests
    {
        [Fact]
        public async Task GetGaleryImagesShouldReturnListOfGaleryViewModel()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var service = new GalleryService(dataMock, mapperMock, null, null);
            var result = await service.GetGaleryImages();

            Assert.IsAssignableFrom<List<GaleryViewModel>>(result);
        }
    }
}