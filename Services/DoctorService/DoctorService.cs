namespace Services.DoctorService
{
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using ViewModels.Doctor;

    public class DoctorService : IDoctorService
    {
        private readonly NeonatologyDbContext data;
        private readonly IMapper mapper;

        public DoctorService(NeonatologyDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task<DoctorProfileViewModel> GetDoctorById(string userId)
            => await this.data.Doctors
            .Where(x => x.Id == userId)
            .ProjectTo<DoctorProfileViewModel>(this.mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        public async Task<string> GetDoctorIdByUserId(string userId)
            => await this.data.Users
                .Where(x => x.Id == userId)
                .Select(x => x.DoctorId)
                .FirstOrDefaultAsync();
    }
}
