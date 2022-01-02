namespace Services.SpecializationService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ViewModels.Doctor;

    public interface ISpecializationService
    {
        Task<ICollection<SpecializationFormModel>> GetAllDoctorSpecializations(string doctorId);
    }
}
