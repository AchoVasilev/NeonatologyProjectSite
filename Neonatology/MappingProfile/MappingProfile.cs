namespace Neonatology.MappingProfile
{
    using AutoMapper;

    using Common;

    using global::Data.Models;

    using ViewModels.Appointments;
    using ViewModels.Chat;
    using ViewModels.City;
    using ViewModels.Doctor;
    using ViewModels.Galery;
    using ViewModels.Offer;
    using ViewModels.Patient;
    using ViewModels.Slot;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<AppointmentCause, AppointmentCauseViewModel>();
            this.CreateMap<Appointment, CreateAppointmentModel>()
                .ForMember(x => x.Start, opt =>
                {
                    opt.MapFrom(y => y.DateTime.ToLocalTime());
                })
                .ForMember(x => x.End, opt =>
                {
                    opt.MapFrom(y => y.End.ToLocalTime());
                });
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
                })
                .ForMember(x => x.DateTime, opt =>
                {
                    opt.MapFrom(y => y.DateTime.ToLocalTime());
                })
                .ForMember(x => x.End, opt =>
                {
                    opt.MapFrom(y => y.End.ToLocalTime());
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

            this.CreateMap<AppointmentSlot, SlotViewModel>()
                .ForMember(x => x.Start, opt =>
                {
                    opt.MapFrom(y => y.Start.ToLocalTime());
                })
                .ForMember(x => x.End, opt =>
                {
                    opt.MapFrom(y => y.End.ToLocalTime());
                });

            this.CreateMap<OfferedService, OfferViewModel>();

            this.CreateMap<Image, UploadImageModel>();

            this.CreateMap<ApplicationUser, ChatConversationsViewModel>()
                .ForMember(x => x.FullName, opt =>
                {
                    opt.MapFrom(y => y.Doctor.FirstName != null ?
                    "Д-р " + y.Doctor.FirstName + " " + y.Doctor.LastName :
                    y.Patient.FirstName + " " + y.Patient.LastName);
                });

            this.CreateMap<ApplicationUser, ChatUserViewModel>()
                .ForMember(x => x.FullName, opt =>
                {
                    opt.MapFrom(y => y.Doctor.FirstName != null ?
                    "Д-р " + y.Doctor.FirstName + " " + y.Doctor.LastName :
                    y.Patient.FirstName + " " + y.Patient.LastName);
                })
                .ForMember(x => x.DoctorEmail, opt =>
                {
                    opt.MapFrom(y => y.Doctor.Email);
                });

            this.CreateMap<Message, ChatMessageWithUserViewModel>()
                .ForMember(vm => vm.CreatedOn, opt =>
                    opt.MapFrom(m => m.CreatedOn.AddHours(3).ToString(GlobalConstants.DateTimeFormats.DateTimeFormat)))
                .ForMember(vm => vm.FullName, opt =>
                    opt.MapFrom(m => m.Sender.Doctor.FirstName != null
                        ? "Д-р " + m.Sender.Doctor.FirstName + " " + m.Sender.Doctor.LastName
                        : m.Sender.Patient.FirstName + " " + m.Sender.Patient.LastName));
        }
    }
}
