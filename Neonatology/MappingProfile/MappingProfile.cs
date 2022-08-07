namespace Neonatology.MappingProfile;

using AutoMapper;
using Data.Models;

using Services.PatientService.Models;

using ViewModels.Address;
using ViewModels.Administration.Appointment;
using ViewModels.Administration.Offer;
using ViewModels.Administration.Rating;
using ViewModels.Administration.User;
using ViewModels.Appointments;
using ViewModels.Chat;
using ViewModels.City;
using ViewModels.Doctor;
using ViewModels.Feedback;
using ViewModels.Gallery;
using ViewModels.Offer;
using ViewModels.Patient;
using ViewModels.Profile;
using ViewModels.Rating;
using ViewModels.Slot;

using static Common.Constants.GlobalConstants.DateTimeFormats;
using GalleryViewModel = ViewModels.Administration.Galery.GalleryViewModel;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateMap<AppointmentCause, AppointmentCauseViewModel>();
        this.CreateMap<Appointment, CreateAppointmentModel>();
        this.CreateMap<Appointment, AppointmentViewModel>()
            .ForMember(x => x.DoctorName, opt =>
            {
                opt.MapFrom(d => string.Join(' ', d.Doctor.FirstName, d.Doctor.LastName));
            })
            //.ForMember(x => x.Address, opt =>
            //{
            //    opt.MapFrom(d => string.Join(", ", d.Doctor.City.Name, d.Doctor.Address));
            //})
            .ForMember(x => x.Rating, opt =>
            {
                opt.MapFrom(d => d.Rating.Number);
            });

        this.CreateMap<Doctor, DoctorProfileViewModel>()
            .ForMember(x => x.UserImageUrl, opt =>
            {
                opt.MapFrom(d => d.User.Image.Url ?? d.User.Image.RemoteImageUrl);
            })
            .ForMember(x => x.FullName, opt =>
            {
                opt.MapFrom(d => string.Join(' ', d.FirstName, d.LastName));
            });
        this.CreateMap<DoctorEditFormModel, DoctorEditModel>();

        this.CreateMap<Address, EditAddressFormModel>();
        this.CreateMap<Address, AddressFormModel>();

        this.CreateMap<Specialization, SpecializationFormModel>();

        this.CreateMap<City, CityFormModel>();

        this.CreateMap<Patient, PatientViewModel>();

        this.CreateMap<AppointmentSlot, SlotViewModel>();

        this.CreateMap<OfferedService, OfferViewModel>();

        this.CreateMap<Image, UploadImageModel>();

        this.CreateMap<ApplicationUser, ChatConversationsViewModel>()
            .ForMember(x => x.FullName, opt =>
            {
                opt.MapFrom(y => y.Doctor.FirstName != null ?
                    "Д-р " + y.Doctor.FirstName + " " + y.Doctor.LastName :
                    y.Patient.FirstName + " " + y.Patient.LastName);
            })
            .ForMember(x => x.UserImageUrl, opt =>
            {
                opt.MapFrom(y => y.Image.Url);
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
                opt.MapFrom(m => m.CreatedOn.AddHours(3).ToString(DateTimeFormat)))
            .ForMember(vm => vm.FullName, opt =>
                opt.MapFrom(m => m.Sender.Doctor.FirstName != null
                    ? "Д-р " + m.Sender.Doctor.FirstName + " " + m.Sender.Doctor.LastName
                    : m.Sender.Patient.FirstName + " " + m.Sender.Patient.LastName));

        this.CreateMap<Patient, ProfileViewModel>();
        this.CreateMap<EditProfileFormModel, EditProfileModel>();
        this.CreateMap<CreateRatingFormModel, CreateRatingModel>();
        this.CreateMap<FeedbackInputModel, CreateFeedbackModel>();
        this.CreateMap<CreateAppointmentModel, CreateAppointmentServiceModel>();
        this.CreateMap<PatientAppointmentCreateModel, CreatePatientAppointmentModel>();
        this.CreateMap<CreateOfferFormModel, CreateOfferServiceModel>();
        this.CreateMap<EditOfferFormModel, EditOfferServiceModel>();
        
        this.CreateMap<Feedback, FeedbackViewModel>()
            .ForMember(x => x.CreatedOn, opt =>
                opt.MapFrom(x => x.CreatedOn.ToString(DateTimeFormat)));

        //Administration
        this.CreateMap<Image, GalleryViewModel>()
            .ForMember(x => x.CreatedOn, opt =>
                opt.MapFrom(y => y.CreatedOn));

        this.CreateMap<Patient, PatientServiceModel>()
            .ForMember(x => x.CreatedOn, opt =>
                opt.MapFrom(y => y.CreatedOn));

        this.CreateMap<PatientServiceModel, UserViewModel>();

        this.CreateMap<Rating, RatingViewModel>()
            .ForMember(x => x.CreatedOn, opt =>
                opt.MapFrom(y => y.CreatedOn.ToLocalTime()));

        this.CreateMap<Appointment, AdminAppointmentViewModel>();
        this.CreateMap<OfferedService, EditOfferFormModel>();
    }
}