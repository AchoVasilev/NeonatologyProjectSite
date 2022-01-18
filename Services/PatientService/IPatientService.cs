namespace Services.PatientService
{
using System.Collections.Generic;
using System.Threading.Tasks;

    using Services.PatientService.Models;

    using ViewModels.Patient;

    public interface IPatientService
    {
        Task CreatePatientAsync(CreatePatientFormModel model, string userId);

        Task<string> GetPatientIdByUserIdAsync(string userId);

        Task<bool> PatientExists(string email);

        Task<PatientViewModel> GetPatientByUserIdAsync(string userId);

        Task<bool> EditPatientAsync(string patientId, CreatePatientFormModel model);

        Task<int> GetPatientsCount();

        Task<int> GetLastThisMonthsRegisteredCount();

        Task<ICollection<PatientServiceModel>> GetAllPatients();
    }
}
