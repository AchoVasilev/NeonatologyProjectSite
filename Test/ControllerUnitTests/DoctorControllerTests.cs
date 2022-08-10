namespace Test.ControllerUnitTests;

using System.Collections.Generic;
using System.Threading.Tasks;
using Helpers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mocks;
using Moq;
using Neonatology.Services.CityService;
using Neonatology.Services.DoctorService;
using Neonatology.ViewModels.City;
using Neonatology.ViewModels.Doctor;
using Neonatology.ViewModels.Slot;
using Neonatology.Web.Controllers;
using Xunit;

public class DoctorControllerTests
{
    [Fact]
    public void DoctorControllerShouldHaveAuthorizeAttribute()
    {
        // Arrange
        var controller = new DoctorController(null, null, null);

        // Act
        var attribute = controller.GetType()
            .GetCustomAttributes(typeof(AuthorizeAttribute), true);

        // Assert 
        Assert.Equal(typeof(AuthorizeAttribute), attribute[0].GetType());
    }

    [Fact]
    public async Task DoctorProfileShouldReturnViewAndCorrectModel()
    {
        var service = new Mock<IDoctorService>();
        service.Setup(x => x.GetDoctorId())
            .ReturnsAsync("1");
        service.Setup(x => x.GetDoctorById("1"))
            .ReturnsAsync(new DoctorProfileViewModel());

        var controller = new DoctorController(service.Object, null, null);
        var result = await controller.Profile();

        var route = Assert.IsType<ViewResult>(result);
        Assert.IsType<DoctorProfileViewModel>(route.Model);
    }

    [Fact]
    public async Task EditShouldReturnViewAndCorrectModel()
    {
        // Arrange
        var doctorService = new Mock<IDoctorService>();
        doctorService.Setup(x => x.GetDoctorId())
            .ReturnsAsync("1");

        var doctor = new DoctorProfileViewModel()
        {
            Id = "1",
            // Address = "Pleven str",
            Age = 30,
            Biography = "asdasdasdasdasdasd",
            Email = "gosho@abv.bg",
            // CityName = "Pleven",
            FullName = "Gosho Peshev",
            PhoneNumber = "098788998",
            UserImageUrl = "asdasdasd",
            YearsOfExperience = 20,
            Specializations = new List<SpecializationFormModel>()
        };

        doctorService.Setup(x => x.GetDoctorById("1"))
            .ReturnsAsync(doctor);

        var cities = new List<CityFormModel>();
        var cityService = new Mock<ICityService>();
        cityService.Setup(x => x.GetAllCities())
            .ReturnsAsync(cities);
        cityService.Setup(x => x.GetCityIdByName("Pleven"))
            .ReturnsAsync(5);

        // Act
        var controller = new DoctorController(doctorService.Object, cityService.Object, null);
        ControllerExtensions.WithIdentity(controller, "1", "gosho", "Doctor");

        var result = await controller.Edit();

        // Assert                 
        var route = Assert.IsType<ViewResult>(result);
        Assert.IsType<DoctorEditFormModel>(route.Model);
    }

    [Fact]
    public async Task EditShouldReturnRedirectToActionProfileWhenSuccessful()
    {
        // Arrange
        var service = new Mock<IDoctorService>();
        var model = new DoctorEditModel()
        {
            //Address = "Pleven",
            Age = 30,
            Biography = "asdasdasda",
            Cities = new List<CityFormModel>(),
            //CityId = 5,
            Email = "gosho@abv.bg",
            FirstName = "Pesho",
            LastName = "Peshev",
            Id = "1",
            PhoneNumber = "098887885"
        };
        
        var controllerModel = new DoctorEditFormModel()
        {
            //Address = "Pleven",
            Age = 30,
            Biography = "asdasdasda",
            Cities = new List<CityFormModel>(),
            //CityId = 5,
            Email = "gosho@abv.bg",
            FirstName = "Pesho",
            LastName = "Peshev",
            Id = "1",
            PhoneNumber = "098887885"
        };

        service.Setup(x => x.EditDoctorAsync(model))
            .ReturnsAsync(true);

        var mapper = MapperMock.Instance;
        // Act
        var controller = new DoctorController(service.Object, null, mapper);
        var result = await controller.Edit(controllerModel);

        // Assert
        var route = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Profile", route.ActionName);
    }

    [Fact]
    public async Task CalendarShouldReturnViewAndModel()
    {
        // Arrange
        var doctorService = new Mock<IDoctorService>();
        var cityService = new Mock<ICityService>();
        var controller = new DoctorController(doctorService.Object, cityService.Object, null);

        // Act
        var result = await controller.Calendar();

        // Assert
        var route = Assert.IsType<ViewResult>(result);
        Assert.IsType<SlotEditModel>(route.Model);
    }
}