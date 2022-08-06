using ViewModels.Doctor;

namespace Test.ServiceUnitTests;

using System.Collections.Generic;
using System.Threading.Tasks;

using Data.Models;

using Services.DoctorService;
using Microsoft.EntityFrameworkCore;
using Mocks;

using Xunit;

public class DoctorServiceTests
{
    [Fact]
    public async Task GetDoctorIdByUserIdShouldReturnId()
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
            },
            Image = new Image()
            {
                Url = "asd.bg"
            }
        };

        await dataMock.Users.AddAsync(user);
        await dataMock.SaveChangesAsync();

        var service = new DoctorService(dataMock, mapperMock, null, null);
        var result = await service.GetDoctorIdByUserId("user");

        Assert.NotNull(result);
        Equals("user", result);
        Assert.IsType<string>(result);
    }

    [Fact]
    public async Task GetDoctorIdByAppointmentIdShouldReturnCorrectId()
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
            },
            Image = new Image()
            {
                Url = "asd.bg"
            }
        };

        var appointmentCause = new AppointmentCause
        {
            Id = 1,
            Name = "Check"
        };

        var appointment = new Appointment
        {
            Id = 1,
            AppointmentCauseId = 1,
            DoctorId = "doc"
        };

        await dataMock.Users.AddAsync(user);
        await dataMock.AppointmentCauses.AddAsync(appointmentCause);
        await dataMock.Appointments.AddAsync(appointment);
        await dataMock.SaveChangesAsync();

        var service = new DoctorService(dataMock, mapperMock, null, null);
        var result = await service.GetDoctorIdByAppointmentId(1);

        Assert.Equal("doc", result);
    }

    [Fact]
    public async Task GetDoctorIdShouldReturnCorrectId()
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
            },
            Image = new Image()
            {
                Url = "asd.bg"
            }
        };

        await dataMock.Users.AddAsync(user);
        await dataMock.SaveChangesAsync();

        var service = new DoctorService(dataMock, mapperMock, null, null);
        var result = await service.GetDoctorId();

        Assert.NotNull(result);
        Assert.Equal("doc", result);
    }

    [Fact]
    public async Task GetDoctorEmailShouldReturnCorrectEmail()
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
                Email = "gosho@gosho.bg"
            },
            Image = new Image()
            {
                Url = "asd.bg"
            }
        };

        await dataMock.Users.AddAsync(user);
        await dataMock.SaveChangesAsync();

        var service = new DoctorService(dataMock, mapperMock, null, null);
        var result = await service.GetDoctorEmail("doc");

        Assert.Equal("gosho@gosho.bg", result);
    }

    [Fact]
    public async Task GetDoctorByIdShouldReturnCorrectModel()
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
                Email = "gosho@gosho.bg"
            },
            Image = new Image()
            {
                Url = "asd.bg"
            }
        };

        await dataMock.Users.AddAsync(user);
        await dataMock.SaveChangesAsync();

        var service = new DoctorService(dataMock, mapperMock, null, null);
        var doctor = await service.GetDoctorById("doc");
            
        Assert.NotNull(doctor);
        Assert.IsType<DoctorProfileViewModel>(doctor);
        Assert.Equal("gosho@gosho.bg", doctor.Email);
    }
        
    [Fact]
    public async Task GetDoctorByUserIdShouldReturnCorrectModel()
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

        var service = new DoctorService(dataMock, mapperMock, null, null);
        var doctor = await service.GetDoctorByUserId("user");
            
        Assert.NotNull(doctor);
        Assert.IsType<DoctorProfileViewModel>(doctor);
        Assert.Equal("gosho@gosho.bg", doctor.Email);
    }

    [Fact]
    public async Task GetDoctorAddressesByIdShouldReturnCorrectCount()
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

        var service = new DoctorService(dataMock, mapperMock, null, null);
        var addressesRes = await service.GetDoctorAddressesById("doc");
            
        Assert.NotNull(addressesRes);
        Assert.Equal(1, addressesRes.Count);
    }

    [Fact]
    public async Task EditDoctorShouldWorkCorrectly()
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

        var service = new DoctorService(dataMock, mapperMock, null, null);

        var model = new DoctorEditModel()
        {
            Id = "doc",
            FirstName = "Mancho",
            LastName = "Goshev",
            Age = 27,
            Email = "gosho@gosho.bg",
        };

        var result = await service.EditDoctorAsync(model);
            
        Assert.True(result.Succeeded);

        var doctor = await dataMock.Doctors.FirstAsync();
            
        Assert.Equal("Mancho", doctor.FirstName);
    }

    [Fact]
    public async Task EditDoctorProfileShouldReturnFalseIfDoctorIsNull()
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

        var service = new DoctorService(dataMock, mapperMock, null, null);

        var model = new DoctorEditModel()
        {
            Id = "asdasd",
            FirstName = "Mancho",
            LastName = "Goshev",
            Age = 27,
            Email = "gosho@gosho.bg",
        };

        var result = await service.EditDoctorAsync(model);
            
        Assert.True(result.Failed);
    }
}