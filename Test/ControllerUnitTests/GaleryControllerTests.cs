namespace Test.ControllerUnitTests
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using Neonatology.Controllers;

    using Services.FileService;

    using ViewModels.Galery;

    using Xunit;

    public class GaleryControllerTests
    {
        [Fact]
        public async Task AllShouldReturnViewAndCorrectModel()
        {
            var service = new Mock<IFileService>();
            service.Setup(x => x.GetGaleryImagesAsync())
                .ReturnsAsync(new UploadImageModel());

            var controller = new GaleryController(service.Object, null, null);
            var result = await controller.All();

            var route = Assert.IsType<ViewResult>(result);
            Assert.IsType<UploadImageModel>(route.Model);
        }
    }
}
