namespace Neonatology.ViewModels.Appointments;

using System;
using Address;
using AutoMapper;
using Common.Mapping;
using Data.Models;

public class AppointmentViewModel : IMapFrom<Appointment>, IMapExplicitly
{
    public int Id { get; set; }

    public DateTime DateTime { get; set; }

    public DateTime End { get; set; }

    public string ParentFirstName { get; set; }

    public string ParentLastName { get; set; }

    public string PhoneNumber { get; set; }

    public string ChildFirstName { get; set; }

    public string AppointmentCauseName { get; set; }

    public string DoctorName { get; set; }

    public bool IsRated { get; set; }

    public int? Rating { get; set; }

    public string RatingComment { get; set; }

    public AddressFormModel Address { get; set; }

    public bool IsConfirmed { get; set; }
    
    public void RegisterMappings(IProfileExpression profile)
    {
        profile.CreateMap<Appointment, AppointmentViewModel>()
            .ForMember(x => x.DoctorName, opt =>
            {
                opt.MapFrom(d => string.Join(' ', d.Doctor.FirstName, d.Doctor.LastName));
            })
            .ForMember(x => x.Rating, opt =>
            {
                opt.MapFrom(d => d.Rating.Number);
            });
    }
}