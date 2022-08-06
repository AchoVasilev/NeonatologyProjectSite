namespace Services.ProfileService;

using System.Threading.Tasks;
using Common;
using ViewModels.Profile;

public interface IProfileService
{
    Task<ProfileViewModel> GetPatientData(string userId);

    Task<OperationResult> EditProfileAsync(EditProfileFormModel model);
}