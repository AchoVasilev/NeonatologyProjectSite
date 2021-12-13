namespace Services.SpecializationService
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;

    using ViewModels.Doctor;

    public class SpecializationService : ISpecializationService
    {
        private readonly NeonatologyDbContext data;
        private readonly IMapper mapper;

        public SpecializationService(NeonatologyDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public ICollection<SpecializationFormModel> GetAllDoctorSpecializations(string doctorId)
            => this.data.Specializations
                   .Where(x => x.DoctorId == doctorId)
                   .ProjectTo<SpecializationFormModel>(this.mapper.ConfigurationProvider)
                   .ToList();
    }
}
