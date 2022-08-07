namespace Services.SpecializationService;

using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using ViewModels.Doctor;

public interface ISpecializationService : ITransientService
{
    Task<ICollection<SpecializationFormModel>> GetAllDoctorSpecializations(string doctorId);
}