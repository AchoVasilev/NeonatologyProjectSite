namespace Test.ServiceUnitTests;

using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Mocks;
using Services.PatientService;
using Services.ProfileService;
using ViewModels.Patient;
using ViewModels.Profile;
using Xunit;

public class ProfileServiceTests
{
    [Fact]
    public async Task GetPatientDataShouldReturnCorrectModel()
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

        await service.CreatePatientAsync(model, "gosho", "rooot");
        var profileService = new ProfileService(dataMock, null, mapperMock, null);

        var patientData = await profileService.GetPatientData("gosho");
        
        Assert.NotNull(patientData);
        Assert.IsType<ProfileViewModel>(patientData);
        
        Assert.Equal("Evlogi", patientData.FirstName);
    }

    [Fact]
    public async Task EditProfileShouldWorkCorrectly()
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

        await service.CreatePatientAsync(model, "gosho", "rooot");
        var profileService = new ProfileService(dataMock, null, mapperMock, null);
        var patient = dataMock.Patients.First();

        var editModel = new EditProfileFormModel()
        {
            Id = patient.Id,
            UserId = user.Id,
            FirstName = "Pesho",
            LastName = "Stamatev",
            Email = "gosho@abv.bg",
            PhoneNumber = "098887889"
        };

        var result = await profileService.EditProfileAsync(editModel);

        Assert.True(result);
    }

    [Fact]
    public async Task EditProfileShouldReturnFalseIfPatientDoestNotExist()
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

        await service.CreatePatientAsync(model, "gosho", "rooot");
        var profileService = new ProfileService(dataMock, null, mapperMock, null);
        var patient = dataMock.Patients.First();

        var editModel = new EditProfileFormModel()
        {
            Id = "fifi",
            UserId = user.Id,
            FirstName = "Pesho",
            LastName = "Stamatev",
            Email = "gosho@abv.bg",
            PhoneNumber = "098887889"
        };

        var result = await profileService.EditProfileAsync(editModel);

        Assert.False(result);
    }
}