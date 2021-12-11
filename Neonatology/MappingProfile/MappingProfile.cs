namespace Neonatology.MappingProfile
{
    using System.Linq;

    using AutoMapper;

    using global::Data.Models;

    using ViewModels.Appointments;
    using ViewModels.City;
    using ViewModels.Doctor;
    using ViewModels.Patient;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Appointment, CreateAppointmentModel>();
            this.CreateMap<Appointment, AppointmentViewModel>()
                .ForMember(x => x.DoctorName, opt =>
                {
                    opt.MapFrom(d => string.Join(' ', d.Doctor.FirstName, d.Doctor.LastName));
                })
                .ForMember(x => x.Address, opt =>
                {
                    opt.MapFrom(d => string.Join(", ", d.Doctor.City.Name, d.Doctor.Address));
                });

            this.CreateMap<Doctor, DoctorProfileViewModel>()
                .ForMember(x => x.ImageUrl, opt =>
                {
                    opt.MapFrom(d => d.Images.Where(x => x.IsDeleted == false).FirstOrDefault().RemoteImageUrl);
                })
                .ForMember(x => x.FullName, opt =>
                {
                    opt.MapFrom(d => string.Join(' ', d.FirstName, d.LastName));
                });

            this.CreateMap<Specialization, SpecializationFormModel>();

            this.CreateMap<City, CityFormModel>();

            this.CreateMap<Patient, PatientViewModel>();
        }
    }
}
