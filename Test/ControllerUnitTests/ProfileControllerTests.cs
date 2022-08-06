namespace Test.ControllerUnitTests;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Moq;

using Neonatology.Controllers;

using Services.CityService;
using Services.ProfileService;

using Helpers;

using ViewModels.City;
using ViewModels.Profile;

using Xunit;

public class ProfileControllerTests
{
    [Fact]
    public void ProfileControllerShouldHaveAuthorizeAttribute()
    {
        var controller = new ProfileController(null, null);
        var actualAttribute = controller.GetType()
            .GetCustomAttributes(typeof(AuthorizeAttribute), true);

        Assert.Equal(typeof(AuthorizeAttribute), actualAttribute[0].GetType());
    }

    [Fact]
    public async Task IndexShouldReturnViewWithCorrectModel()
    {
        var profileService = new Mock<IProfileService>();
        profileService.Setup(x => x.GetPatientData("1"))
            .ReturnsAsync(new ProfileViewModel());

        var profileController = new ProfileController(profileService.Object, null);
        ControllerExtensions.WithIdentity(profileController, "1", "gosho", "Patient");

        var result = await profileController.Index();

        var route = Assert.IsType<ViewResult>(result);
        Assert.IsType<ProfileViewModel>(route.Model);
    }

    [Fact]
    public async Task EditShouldReturnCorrectModelWithView()
    {
        var profileService = new Mock<IProfileService>();
        profileService.Setup(x => x.GetPatientData("1"))
            .ReturnsAsync(new ProfileViewModel() { UserId = "1" });

        var cities = new List<CityFormModel>();
        var cityService = new Mock<ICityService>();
        cityService.Setup(x => x.GetAllCities())
            .ReturnsAsync(cities);

        var profileController = new ProfileController(profileService.Object, cityService.Object);
        ControllerExtensions.WithIdentity(profileController, "1", "gosho", "Patient");

        var result = await profileController.Edit("1");

        var route = Assert.IsType<ViewResult>(result);
        Assert.IsType<EditProfileFormModel>(route.Model);
    }

    [Fact]
    public async Task EditShouldReturnStatusCode404IfPatientDataIsNull()
    {
        var profileService = new Mock<IProfileService>();
        profileService.Setup(x => x.GetPatientData("1"))
            .ReturnsAsync(value: null);

        var profileController = new ProfileController(profileService.Object, null);
        ControllerExtensions.WithIdentity(profileController, "1", "gosho", "Patient");

        var result = await profileController.Edit("1");

        var route = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(404, route.StatusCode);
    }

    [Fact]
    public async Task EditShouldReturnRedirectToActionToIndexIfSuccessful()
    {
        var model = new EditProfileFormModel() { CityId = 1 };
        var profileService = new Mock<IProfileService>();
        profileService.Setup(x => x.EditProfileAsync(model))
            .ReturnsAsync(true);

        var cityService = new Mock<ICityService>();
        cityService.Setup(x => x.GetCityById(1))
            .ReturnsAsync(new CityFormModel());

        var profileController = new ProfileController(profileService.Object, cityService.Object);
        ControllerExtensions.WithIdentity(profileController, "1", "gosho", "Patient");

        var result = await profileController.Edit(model);

        var route = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", route.ActionName);
    }

    [Fact]
    public async Task EditShouldReturnViewWithModelIfEditIsNotSuccessful()
    {
        var model = new EditProfileFormModel();
        var profileService = new Mock<IProfileService>();
        profileService.Setup(x => x.EditProfileAsync(model))
            .ReturnsAsync(false);

        var cities = new List<CityFormModel>();
        var cityService = new Mock<ICityService>();
        cityService.Setup(x => x.GetAllCities())
            .ReturnsAsync(cities);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var profileController = new ProfileController(profileService.Object, cityService.Object)
        {
            TempData = tempData
        };
        ControllerExtensions.WithIdentity(profileController, "1", "gosho", "Patient");

        var result = await profileController.Edit(model);

        var route = Assert.IsType<ViewResult>(result);
        Assert.IsType<EditProfileFormModel>(route.Model);
    }
}