namespace Services.DoctorService
{
    using System.Threading.Tasks;

    using ViewModels.Doctor;

    public interface IDoctorService
    {
        Task<DoctorProfileViewModel> GetDoctorById(string userId);

        Task<string> GetDoctorIdByUserId(string userId);

        Task<string> GetDoctorIdByAppointmentId(int appointmentId);

        Task<bool> UserIsDoctor(string userId);

        Task<string> GetDoctorId();

        Task<bool> EditDoctorAsync(DoctorEditFormModel model);
    }
}
