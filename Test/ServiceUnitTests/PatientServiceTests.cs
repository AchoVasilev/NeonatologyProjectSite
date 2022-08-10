namespace Test.ServiceUnitTests;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mocks;
using Neonatology.Data.Models;
using Neonatology.Services.PatientService;
using Neonatology.ViewModels.Patient;
using Xunit;

public class PatientServiceTests
{
    [Fact]
    public async Task CreatePatientShouldAddPatientToDatabase()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var user = new ApplicationUser
        {
            Id = "gosho",
            UserName = "gosho@abv.bg",
        };

        await dataMock.Users.AddAsync(user);
        await dataMock.SaveChangesAsync();

        var model = new CreatePatientFormModel
        {
            FirstName = "Evlogi",
            LastName = "Penev",
            PhoneNumber = "098789987"
        };

        var service = new PatientService(dataMock, mapperMock);

        await service.CreatePatient(model, "gosho", "rooot");

        Assert.Equal(1, dataMock.Patients.Count());
    }

    [Fact]
    public async Task CreatePatientCreatesModelWithCorrectProperties()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var user = new ApplicationUser
        {
            Id = "gosho",
            UserName = "gosho@abv.bg",
        };

        await dataMock.Users.AddAsync(user);
        await dataMock.SaveChangesAsync();

        var model = new CreatePatientFormModel
        {
            FirstName = "Evlogi",
            LastName = "Penev",
            PhoneNumber = "098789987"
        };

        var service = new PatientService(dataMock, mapperMock);

        await service.CreatePatient(model, "gosho", "rooot");
        var res = await dataMock.Patients.FirstOrDefaultAsync(x => x.UserId == "gosho");

        Assert.Equal("Evlogi", res.FirstName);
        Assert.Equal("Penev", res.LastName);
        Assert.Equal("098789987", res.Phone);
    }

    [Fact]
    public async Task GetPatientItByUserIdShouldReturnCorrectId()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var user = new ApplicationUser
        {
            Id = "gosho",
            UserName = "gosho@abv.bg",
        };

        var patient = new Patient
        {
            Id = "evlogi",
            FirstName = "Evlogi",
            LastName = "Penev",
            Phone = "098789987",
            UserId = "gosho"
        };

        user.Patient = patient;
        await dataMock.Users.AddAsync(user);
        await dataMock.Patients.AddAsync(patient);
        await dataMock.SaveChangesAsync();

        var service = new PatientService(dataMock, mapperMock);
        var result = await service.GetPatientIdByUserId("gosho");

        Assert.Equal("evlogi", result);
    }

    [Fact]
    public async Task GetPatientByUserIdShouldReturnCorrectResult()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var user = new ApplicationUser
        {
            Id = "gosho",
            UserName = "gosho@abv.bg",
        };

        var patient = new Patient
        {
            Id = "evlogi",
            FirstName = "Evlogi",
            LastName = "Penev",
            Phone = "098789987",
            UserId = "gosho"
        };

        user.Patient = patient;
        await dataMock.Users.AddAsync(user);
        await dataMock.Patients.AddAsync(patient);
        await dataMock.SaveChangesAsync();

        var service = new PatientService(dataMock, mapperMock);
        var result = await service.GetPatientByUserId("gosho");

        Assert.NotNull(result);
        Assert.IsType<PatientViewModel>(result);
        Assert.Equal("Evlogi", result.FirstName);
    }

    [Fact]
    public async Task PatientExistsShouldReturnTrueIfExists()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var user = new ApplicationUser
        {
            Id = "gosho",
            UserName = "gosho@abv.bg",
        };

        var patient = new Patient
        {
            Id = "evlogi",
            FirstName = "Evlogi",
            LastName = "Penev",
            Phone = "098789987",
            UserId = "gosho"
        };

        user.Patient = patient;
        await dataMock.Users.AddAsync(user);
        await dataMock.Patients.AddAsync(patient);
        await dataMock.SaveChangesAsync();

        var service = new PatientService(dataMock, mapperMock);
        var result = await service.PatientExists("gosho@abv.bg");

        Assert.True(result);
    }

    [Fact]
    public async Task EditPatientShouldReturnTrueIfSuccessful()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var user = new ApplicationUser
        {
            Id = "gosho",
            UserName = "gosho@abv.bg",
        };

        var patient = new Patient
        {
            Id = "evlogi",
            FirstName = "Evlogi",
            LastName = "Penev",
            Phone = "098789987",
            UserId = "gosho"
        };

        user.Patient = patient;
        await dataMock.Users.AddAsync(user);
        await dataMock.Patients.AddAsync(patient);
        await dataMock.SaveChangesAsync();

        var service = new PatientService(dataMock, mapperMock);

        var model = new CreatePatientFormModel
        {
            FirstName = "Mancho",
            LastName = "Manev",
            PhoneNumber = "098789987"
        };

        var result = await service.EditPatient("evlogi", model);

        Assert.True(result);
    }

    [Fact]
    public async Task EditPatientShouldEditPatientProperties()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var user = new ApplicationUser
        {
            Id = "gosho",
            UserName = "gosho@abv.bg",
        };

        var patient = new Patient
        {
            Id = "evlogi",
            FirstName = "Evlogi",
            LastName = "Penev",
            Phone = "098789987",
            UserId = "gosho"
        };

        user.Patient = patient;
        await dataMock.Users.AddAsync(user);
        await dataMock.Patients.AddAsync(patient);
        await dataMock.SaveChangesAsync();

        var service = new PatientService(dataMock, mapperMock);

        var model = new CreatePatientFormModel
        {
            FirstName = "Mancho",
            LastName = "Manev",
            PhoneNumber = "098789987"
        };

        var result = await service.EditPatient("evlogi", model);
        var editedPatient = await dataMock.Patients
            .FirstOrDefaultAsync(x => x.Id == "evlogi");

        Assert.Equal("Mancho", editedPatient.FirstName);
        Assert.Equal("Manev", editedPatient.LastName);
        Assert.Equal("098789987", editedPatient.Phone);
    }

    [Fact]
    public async Task EditPatientShouldReturnFalseIfPatientIsNotFound()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var user = new ApplicationUser
        {
            Id = "gosho",
            UserName = "gosho@abv.bg",
        };

        var patient = new Patient
        {
            Id = "evlogi",
            FirstName = "Evlogi",
            LastName = "Penev",
            Phone = "098789987",
            UserId = "gosho"
        };

        user.Patient = patient;
        await dataMock.Users.AddAsync(user);
        await dataMock.Patients.AddAsync(patient);
        await dataMock.SaveChangesAsync();

        var service = new PatientService(dataMock, mapperMock);

        var model = new CreatePatientFormModel
        {
            FirstName = "Mancho",
            LastName = "Manev",
            PhoneNumber = "098789987"
        };

        var result = await service.EditPatient("pipi", model);

        Assert.False(result);
    }

    [Fact]
    public async Task EditPatientShouldReturnFalseIfPatientIsDeleted()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var user = new ApplicationUser
        {
            Id = "gosho",
            UserName = "gosho@abv.bg",
        };

        var patient = new Patient
        {
            Id = "evlogi",
            FirstName = "Evlogi",
            LastName = "Penev",
            Phone = "098789987",
            UserId = "gosho",
            IsDeleted = true
        };

        user.Patient = patient;
        await dataMock.Users.AddAsync(user);
        await dataMock.Patients.AddAsync(patient);
        await dataMock.SaveChangesAsync();

        var service = new PatientService(dataMock, mapperMock);

        var model = new CreatePatientFormModel
        {
            FirstName = "Mancho",
            LastName = "Manev",
            PhoneNumber = "098789987"
        };

        var result = await service.EditPatient("evlogi", model);

        Assert.False(result);
    }

    [Fact]
    public async Task GetPatientsCountShouldReturnCorrectCount()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var usersList = new List<ApplicationUser>()
        {
            new ApplicationUser
            {
                Id = "gosho",
                UserName = "gosho@abv.bg",
            },
            new ApplicationUser
            {
                Id = "stamat",
                UserName = "gosho@abv.bg",
            },
            new ApplicationUser
            {
                Id = "mancho",
                UserName = "gosho@abv.bg",
            },
        };

        var patientsList = new List<Patient>()
        {
            new Patient
            {
                Id = "gosho",
                FirstName = "Evlogi",
                LastName = "Penev",
                Phone = "098789987",
                UserId = "gosho",
            },
            new Patient
            {
                Id = "stamat",
                FirstName = "Evlogi",
                LastName = "Penev",
                Phone = "098789987",
                UserId = "stamat",
                IsDeleted = true
            },
            new Patient
            {
                Id = "mancho",
                FirstName = "Evlogi",
                LastName = "Penev",
                Phone = "098789987",
                UserId = "mancho"
            }
        };

        await dataMock.Users.AddRangeAsync(usersList);
        await dataMock.Patients.AddRangeAsync(patientsList);

        await dataMock.SaveChangesAsync();

        var service = new PatientService(dataMock, mapperMock);
        var result = await service.GetPatientsCount();

        Assert.Equal(2, result);
    }

    [Fact]
    public async Task GetThisMonthsRegisteredShouldReturnCorrectCount()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var usersList = new List<ApplicationUser>()
        {
            new ApplicationUser
            {
                Id = "gosho",
                UserName = "gosho@abv.bg",
            },
            new ApplicationUser
            {
                Id = "stamat",
                UserName = "gosho@abv.bg",
            },
            new ApplicationUser
            {
                Id = "mancho",
                UserName = "gosho@abv.bg",
            },
        };

        var patientsList = new List<Patient>()
        {
            new Patient
            {
                Id = "gosho",
                FirstName = "Evlogi",
                LastName = "Penev",
                Phone = "098789987",
                UserId = "gosho",
            },
            new Patient
            {
                Id = "stamat",
                FirstName = "Evlogi",
                LastName = "Penev",
                Phone = "098789987",
                UserId = "stamat",
                IsDeleted = true
            },
            new Patient
            {
                Id = "mancho",
                FirstName = "Evlogi",
                LastName = "Penev",
                Phone = "098789987",
                UserId = "mancho"
            }
        };

        await dataMock.Users.AddRangeAsync(usersList);
        await dataMock.Patients.AddRangeAsync(patientsList);

        await dataMock.SaveChangesAsync();

        var service = new PatientService(dataMock, mapperMock);
        var result = await service.GetLastThisMonthsRegisteredCount();

        Assert.Equal(2, result);
    }

    [Fact]
    public async Task GetPatientByIdShouldReturnCorrectPatient()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var usersList = new List<ApplicationUser>()
        {
            new ApplicationUser
            {
                Id = "gosho",
                UserName = "gosho@abv.bg",
            },
            new ApplicationUser
            {
                Id = "stamat",
                UserName = "gosho@abv.bg",
            },
            new ApplicationUser
            {
                Id = "mancho",
                UserName = "gosho@abv.bg",
            },
        };

        var patientsList = new List<Patient>()
        {
            new Patient
            {
                Id = "gosho",
                FirstName = "Evlogi",
                LastName = "Penev",
                Phone = "098789987",
                UserId = "gosho",
            },
            new Patient
            {
                Id = "stamat",
                FirstName = "Evlogi",
                LastName = "Penev",
                Phone = "098789987",
                UserId = "stamat",
                IsDeleted = true
            },
            new Patient
            {
                Id = "mancho",
                FirstName = "Evlogi",
                LastName = "Penev",
                Phone = "098789987",
                UserId = "mancho"
            }
        };

        await dataMock.Users.AddRangeAsync(usersList);
        await dataMock.Patients.AddRangeAsync(patientsList);

        await dataMock.SaveChangesAsync();

        var service = new PatientService(dataMock, mapperMock);
        var patient = await service.GetPatientById("mancho");

        Assert.NotNull(patient);
        Assert.Equal("Evlogi", patient.FirstName);
    }

    [Fact]
    public async Task HasPaidShouldReturnFalseForUser()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var usersList = new List<ApplicationUser>()
        {
            new ApplicationUser
            {
                Id = "gosho",
                UserName = "gosho@abv.bg",
            },
            new ApplicationUser
            {
                Id = "stamat",
                UserName = "gosho@abv.bg",
            },
            new ApplicationUser
            {
                Id = "mancho",
                UserName = "gosho@abv.bg",
            },
        };

        var patientsList = new List<Patient>()
        {
            new Patient
            {
                Id = "gosho",
                FirstName = "Evlogi",
                LastName = "Penev",
                Phone = "098789987",
                UserId = "gosho",
            },
            new Patient
            {
                Id = "stamat",
                FirstName = "Evlogi",
                LastName = "Penev",
                Phone = "098789987",
                UserId = "stamat",
                IsDeleted = true
            },
            new Patient
            {
                Id = "mancho",
                FirstName = "Evlogi",
                LastName = "Penev",
                Phone = "098789987",
                UserId = "mancho"
            }
        };

        await dataMock.Users.AddRangeAsync(usersList);
        await dataMock.Patients.AddRangeAsync(patientsList);

        await dataMock.SaveChangesAsync();

        var service = new PatientService(dataMock, mapperMock);
        var hasPaid = await service.HasPaid("gosho");

        Assert.False(hasPaid);
    }

    [Fact]
    public async Task GetAllPatientsShouldReturnCorrectCount()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var usersList = new List<ApplicationUser>()
        {
            new ApplicationUser
            {
                Id = "gosho",
                UserName = "gosho@abv.bg",
            },
            new ApplicationUser
            {
                Id = "stamat",
                UserName = "gosho@abv.bg",
            },
            new ApplicationUser
            {
                Id = "mancho",
                UserName = "gosho@abv.bg",
            },
        };

        var patientsList = new List<Patient>()
        {
            new Patient
            {
                Id = "gosho",
                FirstName = "Evlogi",
                LastName = "Penev",
                Phone = "098789987",
                UserId = "gosho",
            },
            new Patient
            {
                Id = "stamat",
                FirstName = "Evlogi",
                LastName = "Penev",
                Phone = "098789987",
                UserId = "stamat",
            },
            new Patient
            {
                Id = "mancho",
                FirstName = "Evlogi",
                LastName = "Penev",
                Phone = "098789987",
                UserId = "mancho"
            }
        };

        await dataMock.Users.AddRangeAsync(usersList);
        await dataMock.Patients.AddRangeAsync(patientsList);

        await dataMock.SaveChangesAsync();

        var service = new PatientService(dataMock, mapperMock);
        var result = await service.GetAllPatients();

        Assert.IsAssignableFrom<List<PatientServiceModel>>(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetPatientIdByEmailShouldReturnCorrectPatient()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var usersList = new List<ApplicationUser>()
        {
            new ApplicationUser
            {
                Id = "gosho",
                UserName = "gosho@abv.bg",
                Email = "gosho@abv.bg",
                Patient = new Patient
                {
                    Id = "gosho",
                    FirstName = "Evlogi",
                    LastName = "Penev",
                    Phone = "098789987",
                    UserId = "gosho",
                }
            },
            new ApplicationUser
            {
                Id = "stamat",
                UserName = "gosho@abv.bg",
                Email = "evlogi@abv.bg",
                Patient = new Patient
                {
                    Id = "stamat",
                    FirstName = "Evlogi",
                    LastName = "Penev",
                    Phone = "098789987",
                    UserId = "stamat",
                    IsDeleted = true
                }
            },
            new ApplicationUser
            {
                Id = "mancho",
                UserName = "gosho@abv.bg",
                Email = "mancho@abv.bg",
                Patient = new Patient
                {
                    Id = "mancho",
                    FirstName = "Evlogi",
                    LastName = "Penev",
                    Phone = "098789987",
                    UserId = "mancho"
                }
            },
        };

        await dataMock.Users.AddRangeAsync(usersList);

        await dataMock.SaveChangesAsync();

        var service = new PatientService(dataMock, mapperMock);
        var patient = await service.GetPatientIdByEmail("gosho@abv.bg");

        Assert.Equal("gosho", patient);
    }

    [Fact]
    public async Task DeletePatientShouldReturnTrueIfPatientIsSuccessfullyDeleted()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var usersList = new List<ApplicationUser>()
        {
            new ApplicationUser
            {
                Id = "gosho",
                UserName = "gosho@abv.bg",
                Email = "gosho@abv.bg",
                Patient = new Patient
                {
                    Id = "gosho",
                    FirstName = "Evlogi",
                    LastName = "Penev",
                    Phone = "098789987",
                    UserId = "gosho",
                }
            },
            new ApplicationUser
            {
                Id = "stamat",
                UserName = "gosho@abv.bg",
                Email = "evlogi@abv.bg",
                Patient = new Patient
                {
                    Id = "stamat",
                    FirstName = "Evlogi",
                    LastName = "Penev",
                    Phone = "098789987",
                    UserId = "stamat",
                    IsDeleted = true
                }
            },
            new ApplicationUser
            {
                Id = "mancho",
                UserName = "gosho@abv.bg",
                Email = "mancho@abv.bg",
                Patient = new Patient
                {
                    Id = "mancho",
                    FirstName = "Evlogi",
                    LastName = "Penev",
                    Phone = "098789987",
                    UserId = "mancho"
                }
            },
        };

        await dataMock.Users.AddRangeAsync(usersList);

        await dataMock.SaveChangesAsync();

        var service = new PatientService(dataMock, mapperMock);
        var patientIsDeleted = await service.DeletePatient("gosho");

        Assert.True(patientIsDeleted);
    }

    [Fact]
    public async Task DeletePatientShouldReturnFalseIfPatientIsNull()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var usersList = new List<ApplicationUser>()
        {
            new ApplicationUser
            {
                Id = "gosho",
                UserName = "gosho@abv.bg",
                Email = "gosho@abv.bg",
                Patient = new Patient
                {
                    Id = "gosho",
                    FirstName = "Evlogi",
                    LastName = "Penev",
                    Phone = "098789987",
                    UserId = "gosho",
                }
            },
            new ApplicationUser
            {
                Id = "stamat",
                UserName = "gosho@abv.bg",
                Email = "evlogi@abv.bg",
                Patient = new Patient
                {
                    Id = "stamat",
                    FirstName = "Evlogi",
                    LastName = "Penev",
                    Phone = "098789987",
                    UserId = "stamat",
                    IsDeleted = true
                }
            },
            new ApplicationUser
            {
                Id = "mancho",
                UserName = "gosho@abv.bg",
                Email = "mancho@abv.bg",
                Patient = new Patient
                {
                    Id = "mancho",
                    FirstName = "Evlogi",
                    LastName = "Penev",
                    Phone = "098789987",
                    UserId = "mancho"
                }
            },
        };

        await dataMock.Users.AddRangeAsync(usersList);

        await dataMock.SaveChangesAsync();

        var service = new PatientService(dataMock, mapperMock);
        var patientIsDeleted = await service.DeletePatient("asdasd");

        Assert.False(patientIsDeleted);
    }
}