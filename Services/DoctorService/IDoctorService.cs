namespace Services.DoctorService;

using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using global::Common.Models;
using ViewModels.Address;
using ViewModels.Doctor;

public interface IDoctorService : ITransientService
{
    Task<DoctorProfileViewModel> GetDoctorById(string doctorId);

    Task<string> GetDoctorIdByUserId(string userId);

    Task<DoctorProfileViewModel> GetDoctorByUserId(string userId);

    Task<string> GetDoctorIdByAppointmentId(int appointmentId);

    Task<string> GetDoctorId();

    Task<OperationResult> EditDoctorAsync(DoctorEditModel model);

    Task<string> GetDoctorEmail(string doctorId);

    Task<ICollection<EditAddressFormModel>> GetDoctorAddressesById(string doctorId);
}