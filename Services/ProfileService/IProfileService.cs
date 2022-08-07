namespace Services.ProfileService;

using System.Threading.Tasks;
using Common;
using global::Common.Models;
using ViewModels.Profile;

public interface IProfileService : ITransientService
{
    Task<ProfileViewModel> GetPatientData(string userId);

    Task<OperationResult> EditProfileAsync(EditProfileModel model);
}