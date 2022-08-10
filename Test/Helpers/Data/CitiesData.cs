namespace Test.Helpers.Data;

using System.Collections.Generic;
using System.Threading.Tasks;
using Neonatology.Data;
using Neonatology.Data.Models;

public static class CitiesData
{
    public static async Task FourCities(NeonatologyDbContext dataMock, bool isDeleted = false)
    {
        var cities = new List<City>()
        {
            new City
            {
                Id = 1,
                Name = "Pleven"
            },
            new City
            {
                Id = 2,
                Name = "Kaspichan"
            },
            new City
            {
                Id = 3,
                Name = "Sofia"
            },
            new City
            {
                Id = 4,
                Name = "Plovdiv",
                IsDeleted = isDeleted
            }
        };
    }
}