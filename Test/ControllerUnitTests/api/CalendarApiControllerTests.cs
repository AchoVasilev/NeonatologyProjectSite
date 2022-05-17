

namespace Test.ControllerUnitTests.api
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Neonatology.Controllers.api;
    using Services.AppointmentCauseService;
    using Services.AppointmentService;
    using Services.PatientService;
    using Services.SlotService;
    using ViewModels.Appointments;
    using ViewModels.Slot;
    using Xunit;

    public class CalendarApiControllerTests
    {
        [Fact]
        public async Task GetCalendarGabrovoSlotsShouldReturnJsonResult()
        {
            var slotServiceMock = new Mock<ISlotService>();
            var model = new List<SlotViewModel>();
            slotServiceMock.Setup(x => x.GetFreeGabrovoSlots())
                .ReturnsAsync(model);

            var controller = new CalendarController(slotServiceMock.Object, null, null, null, null);

            var result = await controller.GetCalendarGabrovoSlots();

            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task GetCalendarPlevenSlotsShouldReturnJsonResult()
        {
            var slotServiceMock = new Mock<ISlotService>();
            var model = new List<SlotViewModel>();
            slotServiceMock.Setup(x => x.GetFreePlevenSlots())
                .ReturnsAsync(model);

            var controller = new CalendarController(slotServiceMock.Object, null, null, null, null);

            var result = await controller.GetCalendarGabrovoSlots();

            Assert.IsType<JsonResult>(result);
        }

        // [Fact]
        // public async Task MakeAppointmentForGuestShouldOkWhenSuccessful()
        // {
        //     var model = new CreateAppointmentModel()
        //     {
        //         Start = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"),
        //         End = DateTime.UtcNow.AddMinutes(10).ToString("dd/MM/yyyy HH:mm"),
        //         Email = "gosho@abv.bg",
        //         ParentFirstName = "Evlogi",
        //         ParentLastName = "Manev",
        //         ChildFirstName = "Pencho",
        //         PhoneNumber = "0988878263",
        //         DoctorId = "1",
        //         AppointmentCauseId = 1,
        //         AddressId = 1
        //     };

        //     var appointmentCauseServiceMock = new Mock<IAppointmentCauseService>();
        //     appointmentCauseServiceMock.Setup(x => x.GetAppointmentCauseByIdAsync(model.AppointmentCauseId))
        //         .ReturnsAsync(new AppointmentCauseViewModel());

        //     var patientServiceMock = new Mock<IPatientService>();
        //     patientServiceMock.Setup(x => x.PatientExists(model.Email))
        //         .ReturnsAsync(false);

        //     var appointmentServiceMock = new Mock<IAppointmentService>();
        //     appointmentServiceMock.Setup(x => x
        //         .AddAsync("1", model, DateTime.ParseExact(model.Start, "dd.MM.yyyy HH:mm", null), DateTime.ParseExact(model.End, "dd.MM.yyyy HH:mm", null)))
        //         .ReturnsAsync(true);

        //     var slotServiceMock = new Mock<ISlotService>();
        //     slotServiceMock.Setup(x => x.DeleteSlotById(1))
        //         .ReturnsAsync(1);

        //     var mailSenderMock = new Mock<IEmailSender>();
        //     mailSenderMock.Setup(x => x.SendEmailAsync("gosho@abv.bg", "asd", "asd"))
        //         .Returns(Task.CompletedTask);

        //     var controller = new CalendarController(slotServiceMock.Object, 
        //         appointmentServiceMock.Object, 
        //         mailSenderMock.Object, 
        //         appointmentCauseServiceMock.Object, 
        //         patientServiceMock.Object);
            
        //     var result = await controller.MakeAnAppointment(model, "1");

        //     Assert.IsType<OkResult>(result);
        // }
    }
}