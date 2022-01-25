namespace Test.Integration
{
using System.Net.Http;
using System.Threading.Tasks;

    using Neonatology;

    using Xunit;

    public class GaleryControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient client;

        public GaleryControllerTests(CustomWebApplicationFactory<Startup> factory)
            => this.client = factory.CreateClient();

        [Fact]
        public async Task AllWhenCalledReturnsAllGaleryPhotos()
        {
            var response = await this.client.GetAsync("/Galery/All");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.Contains("Галерия", responseString);
        }
    }
}
