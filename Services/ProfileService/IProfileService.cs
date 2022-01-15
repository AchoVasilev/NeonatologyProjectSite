namespace Services.ProfileService
{
    using System.Threading.Tasks;

    using ViewModels.Profile;

    public interface IProfileService
    {
        Task<ProfileViewModel> GetPatientData(string userId);
    }
}
