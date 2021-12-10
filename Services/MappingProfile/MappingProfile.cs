namespace Services.MappingProfile
{
    using AutoMapper;

    using Data.Models;

    using ViewModels.Appointments;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Appointment, AppointmentViewModel>();
        }
    }
}
