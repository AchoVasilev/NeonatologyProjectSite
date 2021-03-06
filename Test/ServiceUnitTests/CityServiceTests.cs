namespace Test.ServiceUnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Models;
    using Services.CityService;
    using Mocks;
    using ViewModels.City;
    using Xunit;

    public class CityServiceTests
    {
        [Fact]
        public async Task GetAllCitiesShouldReturnCorrectCount()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

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
                    Name = "Plovdiv"
                }
            };

            await dataMock.Cities.AddRangeAsync(cities);
            await dataMock.SaveChangesAsync();

            var service = new CityService(dataMock, mapperMock);
            var result = await service.GetAllCities();

            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public async Task GetAllCitiesShouldReturnCorrectCountIfOneIsDeleted()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

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
                    IsDeleted = true
                }
            };

            await dataMock.Cities.AddRangeAsync(cities);
            await dataMock.SaveChangesAsync();

            var service = new CityService(dataMock, mapperMock);
            var result = await service.GetAllCities();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetAllCitiesShouldReturnCorrectModel()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

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
                    Name = "Plovdiv"
                }
            };

            await dataMock.Cities.AddRangeAsync(cities);
            await dataMock.SaveChangesAsync();

            var service = new CityService(dataMock, mapperMock);
            var result = await service.GetAllCities();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<ICollection<CityFormModel>>(result);
        }

        [Fact]
        public async Task GetCityIdByNameShouldReturnCorrectId()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

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
                    Name = "Plovdiv"
                }
            };

            await dataMock.Cities.AddRangeAsync(cities);
            await dataMock.SaveChangesAsync();

            var service = new CityService(dataMock, mapperMock);
            var result = await service.GetCityIdByName("Pleven");

            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetCityByIdShouldReturnCorrectModel()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

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
                    Name = "Plovdiv"
                }
            };

            await dataMock.Cities.AddRangeAsync(cities);
            await dataMock.SaveChangesAsync();

            var service = new CityService(dataMock, mapperMock);
            var result = await service.GetCityById(1);

            Assert.Equal(1, result.Id);
            Assert.IsType<CityFormModel>(result);
        }

        [Fact]
        public async Task GetDoctorAddressesShouldReturnCorrectCount()
        {
            var doctor = new Doctor
            {
                Id = "doc",
                FirstName = "Pesho",
                LastName = "Peshev",
                Age = 30,
                Addresses = new List<Address>()
                {
                    new Address()
                    {
                        CityId = 1,
                        City = new City()
                        {
                            Name = "Pleven"
                        },
                        DoctorId = "doc",
                        StreetName = "Kaspichan"
                    }
                }
            };

            var dataMock = DatabaseMock.Instance;
            await dataMock.Doctors.AddAsync(doctor);
            await dataMock.SaveChangesAsync();

            var mapperMock = MapperMock.Instance;
            
            var service = new CityService(dataMock, mapperMock);

            var cities = await service.GetDoctorAddressesByDoctorId("doc");
            
            Assert.Equal(1, cities.Count);
        }
    }
}