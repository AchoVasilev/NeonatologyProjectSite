namespace Neonatology.MappingProfile
{
    using AutoMapper;

    using global::Data.Models;

    using ViewModels.Appointments;
    using ViewModels.City;
    using ViewModels.Doctor;
    using ViewModels.Patient;
    using ViewModels.Slot;

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
                })
                .ForMember(x => x.Rating, opt =>
                {
                    opt.MapFrom(d => d.Rating.Number);
                });

            this.CreateMap<Doctor, DoctorProfileViewModel>()
                .ForMember(x => x.ImageUrl, opt =>
                {
                    opt.MapFrom(d => d.Image.Url ?? d.Image.RemoteImageUrl);
                })
                .ForMember(x => x.FullName, opt =>
                {
                    opt.MapFrom(d => string.Join(' ', d.FirstName, d.LastName));
                });

            this.CreateMap<Specialization, SpecializationFormModel>();

            this.CreateMap<City, CityFormModel>();

            this.CreateMap<Patient, PatientViewModel>();

            this.CreateMap<AppointmentSlot, SlotViewModel>();
        }
    }
}
