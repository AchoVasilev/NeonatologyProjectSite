namespace Services.MappingProfile
{
    using System.Linq;

    using AutoMapper;

    using Data.Models;

    using ViewModels.Appointments;
    using ViewModels.Doctor;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Appointment, AppointmentViewModel>();

            this.CreateMap<Specialization, SpecializationFormModel>();

            this.CreateMap<Doctor, DoctorProfileViewModel>()
                .ForMember(x => x.ImageUrl, opt =>
                {
                    opt.MapFrom(d => d.Images.Where(x => x.IsDeleted == false).FirstOrDefault().RemoteImageUrl);
                })
                .ForMember(x => x.FullName, opt =>
                {
                    opt.MapFrom(d => d.FirstName + ' ' + d.LastName);
                });
        }
    }
}
