namespace Services.PatientService;

using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Data.Models;

using Models;

using ViewModels.Patient;

public interface IPatientService
{
    Task CreatePatient(CreatePatientFormModel model, string userId, string webRootPath);

    Task<string> GetPatientIdByUserId(string userId);

    Task<bool> PatientExists(string email);

    Task<PatientViewModel> GetPatientByUserId(string userId);

    Task<OperationResult> PatientIsRegistered(string userId);

    Task<bool> EditPatient(string patientId, CreatePatientFormModel model);

    Task<int> GetPatientsCount();

    Task<int> GetLastThisMonthsRegisteredCount();

    Task<Patient> GetPatientById(string patientId);

    Task<ICollection<PatientServiceModel>> GetAllPatients();

    Task<string> GetPatientIdByEmail(string email);

    Task<bool> HasPaid(string userId);

    Task<bool> DeletePatient(string userId);
}