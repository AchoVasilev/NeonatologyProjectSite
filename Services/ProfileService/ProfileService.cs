namespace Services.ProfileService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using ViewModels.Profile;

    public class ProfileService : IProfileService
    {
        private readonly NeonatologyDbContext data;
        private readonly IMapper mapper;

        public ProfileService(NeonatologyDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task<ProfileViewModel> GetPatientData(string userId)
        {
            var patientData = await this.data.Patients
                 .Where(x => x.UserId == userId && x.IsDeleted == false)
                 .ProjectTo<ProfileViewModel>(this.mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync();

            return patientData;
        }
    }
}
