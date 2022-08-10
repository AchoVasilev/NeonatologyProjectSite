namespace Test.ServiceUnitTests;

using System.Collections.Generic;
using System.Threading.Tasks;
using Mocks;
using Neonatology.Data.Models;
using Neonatology.Services.UserService;
using Xunit;

public class UserServiceTests
{
    [Fact]
    public async Task GetUserIdByDoctorIdShouldWorkCorrectly()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;
        var addresses = new List<Address>
        {
            new Address
            {
                City = new City
                {
                    Name = "Pleven"
                },

                StreetName = "Kaspichan Str"
            }
        };

        var user = new ApplicationUser()
        {
            Id = "user",
            Doctor = new Doctor()
            {
                Id = "doc",
                FirstName = "Gosho",
                LastName = "Goshev",
                Age = 27,
                Addresses = addresses,
                Email = "gosho@gosho.bg",
                UserId = "user"
            },
            Image = new Image()
            {
                Url = "asd.bg"
            }
        };

        await dataMock.Users.AddAsync(user);
        await dataMock.SaveChangesAsync();

        var service = new UserService(dataMock);
        var result = await service.GetUserIdByDoctorIdAsync("doc");

        Assert.Equal("user", result);
    }

    [Fact]
    public async Task GetUserByIdAsyncShouldReturnCorrectUserAndModel()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;
        var addresses = new List<Address>
        {
            new Address
            {
                City = new City
                {
                    Name = "Pleven"
                },

                StreetName = "Kaspichan Str"
            }
        };

        var user = new ApplicationUser()
        {
            Id = "user",
            Doctor = new Doctor()
            {
                Id = "doc",
                FirstName = "Gosho",
                LastName = "Goshev",
                Age = 27,
                Addresses = addresses,
                Email = "gosho@gosho.bg",
                UserId = "user"
            },
            Image = new Image()
            {
                Url = "asd.bg"
            }
        };

        await dataMock.Users.AddAsync(user);
        await dataMock.SaveChangesAsync();

        var service = new UserService(dataMock);
        var result = await service.GetUserByIdAsync("user");
            
        Assert.NotNull(result);
        Assert.IsType<ApplicationUser>(result);
            
        Assert.Equal("user", result.Id);
    }

    [Fact]
    public async Task FindByUsernameShouldWorkCorrectly()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;
        var addresses = new List<Address>
        {
            new Address
            {
                City = new City
                {
                    Name = "Pleven"
                },

                StreetName = "Kaspichan Str"
            }
        };

        var user = new ApplicationUser()
        {
            Id = "user",
            UserName = "gosho",
            Doctor = new Doctor()
            {
                Id = "doc",
                FirstName = "Gosho",
                LastName = "Goshev",
                Age = 27,
                Addresses = addresses,
                Email = "gosho@gosho.bg",
                UserId = "user"
            },
            Image = new Image()
            {
                Url = "asd.bg"
            }
        };

        await dataMock.Users.AddAsync(user);
        await dataMock.SaveChangesAsync();

        var service = new UserService(dataMock);

        var result = await service.FindByUserNameAsync("gosho");
            
        Assert.NotNull(result);
        Assert.IsType<ApplicationUser>(result);
        Assert.Equal("user", result.Id);
    }
}