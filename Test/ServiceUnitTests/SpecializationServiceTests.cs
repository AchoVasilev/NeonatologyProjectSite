namespace Test.ServiceUnitTests;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Constants;
using Data.Models;
using Data.Models.Dto;
using Mocks;
using Services.SpecializationService;
using ViewModels.Doctor;
using Xunit;

public class SpecializationServiceTests
{
    [Fact]
    public async Task GetAllSpecializationsShouldReturnCorrectCount()
    {
        var specializations = new List<SpecializationDto>();
        specializations.Add(new SpecializationDto
        {
            Name = GlobalConstants.DoctorConstants.NeonatologySpecializationName,
            Description = GlobalConstants.DoctorConstants.NeonatologySpecializationDescription,
        });

        specializations.Add(new SpecializationDto
        {
            Name = GlobalConstants.DoctorConstants.ChildSicknessName,
            Description = GlobalConstants.DoctorConstants.ChildSicknessDescription
        });

        var dataMock = DatabaseMock.Instance;
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

        var doctor = dataMock.Doctors.First();
        foreach (var specializationDto in specializations)
        {
            var specialization = new Specialization()
            {
                Name = specializationDto.Name,
                Description = specializationDto.Description,
                DoctorId = doctor.Id,
                Doctor = doctor
            };

            await dataMock.AddAsync(specialization);
        }

        await dataMock.SaveChangesAsync();

        var mapper = MapperMock.Instance;

        var service = new SpecializationService(dataMock, mapper);
        var result = await service.GetAllDoctorSpecializations("doc");
        
        Assert.NotNull(result);
        Assert.IsAssignableFrom<List<SpecializationFormModel>>(result);
        
        Assert.Equal(2, result.Count);
    }
}