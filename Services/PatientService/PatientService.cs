namespace Services.PatientService
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;
    using Data.Models;

    using Microsoft.EntityFrameworkCore;

    using Services.PatientService.Models;

    using ViewModels.Patient;

    using static Common.GlobalConstants.FileConstants;

    public class PatientService : IPatientService
    {
        private readonly NeonatologyDbContext data;
        private readonly IMapper mapper;

        public PatientService(NeonatologyDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task CreatePatientAsync(CreatePatientFormModel model, string userId, string webRootPath)
        {
            var user = await this.data.Users
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            var patient = new Patient()
            {
                UserId = userId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.PhoneNumber,
            };

            var url = Path.GetRelativePath(webRootPath, NoProfilePicUrl);
            url = url.Replace('\\', '/');

            user.Image = new Image()
            {
                Url = url,
                Name = "NoAvatarProfileImage.png",
                Extension = "png"
            };
            user.Patient = patient;
            user.PhoneNumber = model.PhoneNumber;

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

        public async Task<bool> PatientExists(string email)
            => await this.data.Users
                        .AnyAsync(x => x.UserName == email);

        public async Task<Patient> GetPatientById(string patientId)
              => await this.data.Patients
                      .FirstOrDefaultAsync(x => x.Id == patientId);

        public async Task<bool> EditPatientAsync(string patientId, CreatePatientFormModel model)
        {
            var patient = await this.data.Patients
                .FirstOrDefaultAsync(x => x.Id == patientId && x.IsDeleted == false);

            if (patient == null)
            {
                return false;
            }

            patient.Phone = model.PhoneNumber;
            patient.FirstName = model.FirstName;
            patient.LastName = model.LastName;
            patient.ModifiedOn = DateTime.UtcNow;

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetPatientsCount()
            => await this.data.Patients
                        .Where(x => x.IsDeleted == false)
                        .CountAsync();

        public async Task<int> GetLastThisMonthsRegisteredCount()
            => await this.data.Patients
                        .Where(x => x.IsDeleted == false && 
                                x.CreatedOn.Month == DateTime.Now.Month)
                        .CountAsync();

        public async Task<ICollection<PatientServiceModel>> GetAllPatients()
            => await this.data.Patients
                        .Where(x => x.IsDeleted == false)
                        .ProjectTo<PatientServiceModel>(this.mapper.ConfigurationProvider)
                        .ToListAsync();

        public async Task<string> GetPatientIdByEmail(string email)
            => await this.data.Users
                        .Where(x => x.Email == email)
                        .Select(x => x.PatientId)
                        .FirstOrDefaultAsync();
    }
}
