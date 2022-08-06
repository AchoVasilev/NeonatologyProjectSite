namespace Test.ControllerUnitTests;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Neonatology.Controllers;

using Services.OfferService;

using ViewModels.Offer;

using Xunit;

public class OfferControllerTests
{
    [Fact]
    public async Task AllShouldReturnViewWithModel()
    {
        var feedbacks = new List<OfferViewModel>();
        var serviceMock = new Mock<IOfferService>();
        serviceMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(feedbacks);

        var controller = new OfferController(serviceMock.Object);

        var result = await controller.All();

        var route = Assert.IsType<ViewResult>(result);

        Assert.IsAssignableFrom<ICollection<OfferViewModel>>(route.Model);
    }
}