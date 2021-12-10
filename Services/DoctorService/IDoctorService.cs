namespace Services.DoctorService
{
    using System.Threading.Tasks;

    using ViewModels.Doctor;

    public interface IDoctorService
    {
        Task<DoctorProfileViewModel> GetDoctorById(string userId);

        Task<DoctorProfileViewModel> DoctoryById(string userId);
    }
}
