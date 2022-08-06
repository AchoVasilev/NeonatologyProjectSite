namespace Services.ProfileService;

using System.Threading.Tasks;
using Common;
using Common.Models;
using ViewModels.Profile;

public interface IProfileService
{
    Task<ProfileViewModel> GetPatientData(string userId);

    Task<OperationResult> EditProfileAsync(EditProfileModel model);
}