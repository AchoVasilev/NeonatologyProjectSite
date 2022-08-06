namespace Test.ControllerUnitTests.api;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Neonatology.Controllers.api;
using Services.AppointmentService;
using Services.SlotService;
using ViewModels.Appointments;
using ViewModels.Slot;
using Xunit;

public class DoctorCalendarApiControllerTests
{
    [Fact]
    public async Task GetDoctorGabrovoAppointmentsShouldReturnJsonResult()
    {
        var model = new List<AppointmentViewModel>();
        var appointmentServiceMock = new Mock<IAppointmentService>();
        appointmentServiceMock.Setup(x => x.GetGabrovoAppointments())
            .ReturnsAsync(model);

        var controller = new DoctorCalendarController(appointmentServiceMock.Object, null);
        var result = await controller.GetDoctorGabrovoAppointments();

        Assert.IsType<JsonResult>(result);
    }

    [Fact]
    public async Task GetDoctorPlevenAppointmentsShouldReturnJsonResult()
    {
        var model = new List<AppointmentViewModel>();
        var appointmentServiceMock = new Mock<IAppointmentService>();
        appointmentServiceMock.Setup(x => x.GetPlevenAppointments())
            .ReturnsAsync(model);

        var controller = new DoctorCalendarController(appointmentServiceMock.Object, null);
        var result = await controller.GetDoctorPlevenAppointments();

        Assert.IsType<JsonResult>(result);
    }

    [Fact]
    public async Task GetCalendarGabrovoSlotsShouldReturnJsonResult()
    {
        var model = new List<SlotViewModel>();
        var slotServiceMock = new Mock<ISlotService>();
        slotServiceMock.Setup(x => x.GetGabrovoSlots())
            .ReturnsAsync(model);

        var controller = new DoctorCalendarController(null, slotServiceMock.Object);
        var result = await controller.GetCalendarGabrovoSlots();

        Assert.IsType<JsonResult>(result);
    }

    [Fact]
    public async Task GetCalendarPlevenSlotsShouldReturnJsonResult()
    {
        var model = new List<SlotViewModel>();
        var slotServiceMock = new Mock<ISlotService>();
        slotServiceMock.Setup(x => x.GetPlevenSlots())
            .ReturnsAsync(model);

        var controller = new DoctorCalendarController(null, slotServiceMock.Object);
        var result = await controller.GetCalendarGabrovoSlots();

        Assert.IsType<JsonResult>(result);
    }

    [Fact]
    public async Task EditSlotShouldReturnOkResultWhenSuccessful()
    {
        var slot = new SlotEditModel()
        {
            Id = 1,
            Text = "asdasdasd",
            Status = "Зает",
            AddressId = 1
        };

        var slotServiceMock = new Mock<ISlotService>();
        slotServiceMock.Setup(x => x.EditSlot(slot.Id, slot.Status, slot.Text))
            .ReturnsAsync(true);

        var controller = new DoctorCalendarController(null, slotServiceMock.Object);
        var result = await controller.EditSlot(slot);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task EditSlotShouldReturnBadRequestResultWhenNotSuccessful()
    {
        var slot = new SlotEditModel()
        {
            Id = 1,
            Text = "asdasdasd",
            Status = "Зает",
            AddressId = 1
        };

        var slotServiceMock = new Mock<ISlotService>();
        slotServiceMock.Setup(x => x.EditSlot(slot.Id, slot.Status, slot.Text))
            .ReturnsAsync(false);

        var controller = new DoctorCalendarController(null, slotServiceMock.Object);
        var result = await controller.EditSlot(slot);

        Assert.IsType<BadRequestObjectResult>(result);
    }
}