namespace IntegrationTests
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Testing;

    using Neonatology;

    using Xunit;

    public class HomeControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public HomeControllerTests(WebApplicationFactory<Startup> factory) 
            => this.factory = factory;

        [Theory]
        [InlineData("/")]
        [InlineData("/Index")]
        public async Task IndexShouldReturnSuccessAndCorrectContentType(string url)
        {
            //Arrange
            var client = this.factory.CreateClient();

            //Act
            var response = await client.GetAsync(url);

            //Assert
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.Contains("Тук можете да запишете вашия час за преглед и да получите консултация", html);
        }

        [Fact]
        public async Task PrivacyShouldReturnSuccessAndCorrectContentType()
        {
            var client = this.factory.CreateClient();

            var response = await client.GetAsync("/Home/Privacy");

            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();

            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.Contains("Какви данни събираме:", html);
        }
    }
}
