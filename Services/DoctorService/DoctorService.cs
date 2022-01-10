namespace Services.DoctorService
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using CloudinaryDotNet;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using Services.ImageService;

    using ViewModels.Doctor;

    public class DoctorService : IDoctorService
    {
        private readonly NeonatologyDbContext data;
        private readonly IImageService imageService;
        private readonly IMapper mapper;
        private readonly Cloudinary cloudinary;

        public DoctorService(NeonatologyDbContext data, IMapper mapper, IImageService imageService, Cloudinary cloudinary)
        {
            this.data = data;
            this.mapper = mapper;
            this.imageService = imageService;
            this.cloudinary = cloudinary;
        }

        public async Task<DoctorProfileViewModel> GetDoctorById(string doctorId)
            => await this.data.Doctors
            .Where(x => x.Id == doctorId)
            .ProjectTo<DoctorProfileViewModel>(this.mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        public async Task<DoctorProfileViewModel> GetDoctorByUserId(string userId)
            => await this.data.Doctors
            .Where(x => x.UserId == userId)
            .ProjectTo<DoctorProfileViewModel>(this.mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        public async Task<string> GetDoctorIdByUserId(string userId)
            => await this.data.Users
                .Where(x => x.Id == userId)
                .Select(x => x.DoctorId)
                .FirstOrDefaultAsync();

        public async Task<string> GetDoctorIdByAppointmentId(int appointmentId)
            => await this.data.Appointments
                        .Where(x => x.Id == appointmentId && x.IsDeleted == false)
                        .Select(x => x.DoctorId)
                        .FirstOrDefaultAsync();

        public async Task<bool> UserIsDoctor(string userId)
            => await this.data.Users
                          .AnyAsync(x => x.Id == userId && x.DoctorId != null);

        public async Task<string> GetDoctorId()
            => await this.data.Doctors
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();

        public async Task<string> GetDoctorEmail(string doctorId)
            => await this.data.Doctors
                         .Where(x => x.Id == doctorId)
                         .Select(x => x.Email)
                         .FirstOrDefaultAsync();

        public async Task<bool> EditDoctorAsync(DoctorEditFormModel model)
        {
            var doctor = await this.data.Doctors.FindAsync(model.Id);

            if (doctor == null)
            {
                return false;
            }

            doctor.FirstName = model.FirstName;
            doctor.LastName = model.LastName;
            doctor.PhoneNumber = model.PhoneNumber;
            doctor.YearsOfExperience = model.YearsOfExperience;
            doctor.Image.RemoteImageUrl = model.ImageUrl;
            doctor.Age = model.Age;
            doctor.Biography = model.Biography;
            doctor.CityId = model.CityId;

            doctor.Image.IsDeleted = true;
            doctor.Image.DeletedOn = DateTime.UtcNow;

            await this.imageService.UploadImage(cloudinary, model.Picture, doctor);
            await this.data.SaveChangesAsync();

            return true;
        }
    }
}
