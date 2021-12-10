namespace Services.DoctorService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;
    using Data.Models;

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

        public async Task<DoctorProfileViewModel> DoctoryById(string userId)
        {
            var model = await this.data.Doctors
                .Where(x => x.Id == userId)
                .Select(x => new DoctorProfileViewModel
                {
                    FullName = string.Join(' ', x.FirstName, x.LastName),
                    Address = x.Address,
                    CityName = x.City.Name,
                    Specializations = x.Specializations.Select(s => new SpecializationFormModel
                    {
                        Name = s.Name,
                        Description = s.Description
                    }).ToList(),
                    Age = x.Age,
                    ImageUrl = x.Images.Select(x => x.RemoteImageUrl).First(),
                    Biography = x.Biography,
                    Email = x.Email,
                    UserId = x.UserId,
                    Id = x.Id,
                    PhoneNumber = x.PhoneNumber,
                    YearsOfExperience = x.YearsOfExperience
                }).FirstOrDefaultAsync();

            return model;
        }
    }
}
