namespace Services.SpecializationService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;

    using Microsoft.EntityFrameworkCore;

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

        public async Task<ICollection<SpecializationFormModel>> GetAllDoctorSpecializations(string doctorId)
            => await this.data.Specializations
                           .Where(x => x.DoctorId == doctorId)
                           .ProjectTo<SpecializationFormModel>(this.mapper.ConfigurationProvider)
                           .ToListAsync();
    }
}
