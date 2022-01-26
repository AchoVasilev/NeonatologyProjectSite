namespace IntegrationTests
{
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Testing;

    using Neonatology;

    using Xunit;

    public class DoctorControllerTests
    {
        [Fact]
        public async Task ProfileShouldReturnCorrectHeaderAndSuccess()
        {
            var factory = new WebApplicationFactory<Startup>();
            var client = factory.CreateClient();

            var response = await client.GetAsync("/Doctor/Profile");
            var html = await response.Content.ReadAsStringAsync();

            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.Contains("Професионален профил", html);
        }

        [Theory]
        [InlineData("/Doctor/Edit")]
        [InlineData("/Doctor/Calendar")]
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
