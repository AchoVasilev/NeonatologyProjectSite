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
        [InlineData("/Appointment/MakeAnAppointment")]
        public async Task AppointmentCreatingShouldReturnCorrectHeadersAndSuccess(string url)
        {
            var factory = new WebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.Contains("Запази час", html);
        }

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

        [Fact]
        public async Task MakePatientAppointmentShouldReturnCorrectHeadersAndSuccess()
        {
            var factory = new WebApplicationFactory<Startup>();
            var provider = TestClaimsProvider.WithUserClaims();
            var client = factory.CreateClientWithTestAuth(provider);

            var response = await client.GetAsync("/Appointment/MakePatientAppointment");
            var html = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("Запази час", html);
        }
    }
}
