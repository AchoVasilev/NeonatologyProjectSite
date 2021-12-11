﻿namespace Services.PatientService
{
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;
    using Data.Models;

    using Microsoft.EntityFrameworkCore;

    using ViewModels.Patient;

    public class PatientService : IPatientService
    {
        private readonly NeonatologyDbContext data;
        private readonly IMapper mapper;

        public PatientService(NeonatologyDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task CreatePatientAsync(CreatePatientFormModel model, string userId)
        {
            var user = await this.data.Users
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            var patient = new Patient()
            {
                UserId = userId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.PhoneNumber
            };

            user.Patient = patient;

            await this.data.Patients.AddAsync(patient);

            await this.data.SaveChangesAsync();
        }

        public async Task<string> GetPatientIdByUserIdAsync(string userId)
           => await this.data.Patients
                .Where(x => x.UserId == userId && x.IsDeleted == false)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

        public async Task<PatientViewModel> GetPatientByUserIdAsync(string userId)
            => await this.data.Patients
                  .Where(x => x.UserId == userId && x.IsDeleted == false)
                  .ProjectTo<PatientViewModel>(this.mapper.ConfigurationProvider)
                  .FirstOrDefaultAsync();
    }
}
