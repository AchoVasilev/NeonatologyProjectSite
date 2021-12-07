namespace Data.Seeding
{
    using System.Collections.Generic;
using System.Threading.Tasks;

    using Data.Models.Dto;

    using Data.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using Newtonsoft.Json;
    using System.IO;

    public static class ApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> PrepareDatabase(this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var serviceProvider = scopedServices.ServiceProvider;
            var data = serviceProvider.GetRequiredService<NeonatologyDbContext>();

            data.Database.Migrate();

            await SeedCities(data);

            return app;
        }

        private static async Task SeedCities(NeonatologyDbContext data)
        {
            var citiesJson = File.ReadAllText("Datasets/Cities.json");
            var citiesDto = JsonConvert.DeserializeObject<CityDto[]>(citiesJson);
            var cities = new List<City>();

            foreach (var cityDto in citiesDto)
            {
                var city = new City()
                {
                    Name = cityDto.Name,
                    ZipCode = cityDto.ZipCode
                };

                cities.Add(city);
            }

            await data.Cities.AddRangeAsync(cities);
            await data.SaveChangesAsync();
        }
    }
}
