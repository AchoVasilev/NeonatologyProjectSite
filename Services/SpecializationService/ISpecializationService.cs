namespace Services.SpecializationService
{
    using System.Collections.Generic;

    using ViewModels.Doctor;

    public interface ISpecializationService
    {
        ICollection<SpecializationFormModel> GetAllDoctorSpecializations(string doctorId);
    }
}
