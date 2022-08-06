namespace Test.AdministrationArea.Controllers;

using System.Reflection;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Neonatology.Areas.Administration.Controllers;

using Xunit;

public class BaseControllerTests
{
    [Fact]
    public void ControllerShouldHaveAuthorizeAttribute()
    {
        var controller = new BaseController();
        var attribute = controller.GetType()
            .GetCustomAttribute(typeof(AuthorizeAttribute), true) as AuthorizeAttribute;

        Assert.Equal(typeof(AuthorizeAttribute), attribute.GetType());
        Assert.Contains("Administrator", attribute.Roles);
    }

    [Fact]
    public void ControllerShouldHaveAreaAttribute()
    {
        var controller = new BaseController();
        var attribute = controller.GetType()
            .GetCustomAttribute(typeof(AreaAttribute), true) as AreaAttribute;

        Assert.Equal(typeof(AreaAttribute), attribute.GetType());
        Assert.Contains("Administration", attribute.RouteValue);
    }
}