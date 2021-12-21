namespace Services.PatientService
{
    using System.Threading.Tasks;

    using ViewModels.Patient;

    public interface IPatientService
    {
        Task CreatePatientAsync(CreatePatientFormModel model, string userId);

        Task<string> GetPatientIdByUserIdAsync(string userId);

        Task<PatientViewModel> GetPatientByUserIdAsync(string userId);

        Task<bool> EditPatientAsync(string patientId, CreatePatientFormModel model);
    }
}
