namespace IntegrationTests;

using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Neonatology.Web;
using Xunit;

public class AppointmentControllerTests
{ 
    [Theory]
    [InlineData("/Appointment/MyPastAppointments")]
    [InlineData("/Appointment/MyUpcomingAppointments")]
    [InlineData("/Appointment/DoctorPastAppointments")]
    [InlineData("/Appointment/DoctorUpcomingAppointments")]
    [InlineData("/Appointment/TodaysAppointments")]
    [InlineData("/Appointment/MakePatientAppointment")]
    public async Task AuthorizeOverTheseMethodsShouldReturnRedirectStatusCodeIfUserIsAnnonymous(string url)
    {
        var factory = new WebApplicationFactory<Startup>();
        var client = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

        var response = await client.GetAsync(url);

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Identity/Account/Login", response.Headers.Location?.OriginalString);
    }
}