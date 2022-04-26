namespace IntegrationTests
{
    using System.Net;
    using System.Threading.Tasks;

    using IntegrationTests.Helpers;

    using Microsoft.AspNetCore.Mvc.Testing;

    using Neonatology;

    using Xunit;

    public class AppointmentControllerTests
    { 
        [Theory]
        [InlineData("/Appointment/MyAppointments")]
        [InlineData("/Appointment/DoctorAppointments")]
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
            Assert.Contains("/Identity/Account/Login", response.Headers.Location.OriginalString);
        }
    }
}
