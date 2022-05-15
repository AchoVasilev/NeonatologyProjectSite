namespace Test.ControllerUnitTests;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Neonatology.Controllers;
using Services.SlotService;
using ViewModels.Slot;
using Xunit;

public class SlotControllerTests
{
    [Fact]
    public async Task TodaySlotsShouldReturnCorrectModel()
    {
        var slots = new List<SlotViewModel>()
        {
            new SlotViewModel()
            {
                AddressId = 1,
                AddressCityId = 1,
                AddressCityName = "Pleven",
                End = DateTime.UtcNow.AddMinutes(10),
                Id = 1,
                Start = DateTime.UtcNow,
                Status = "Зает",
                Text = "adsasdasd"
            }
        };
        
        var slotService = new Mock<ISlotService>();
        slotService.Setup(x => x.GetTodaysTakenSlots())
            .ReturnsAsync(slots);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new SlotController(slotService.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Doctor");

        var result = await controller.TodaySlots();

        var route = Assert.IsType<ViewResult>(result);
    }
}