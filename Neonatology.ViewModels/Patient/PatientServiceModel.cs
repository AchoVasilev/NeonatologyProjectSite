namespace Neonatology.ViewModels.Patient;

using System;
using AutoMapper;
using Common.Mapping;
using Data.Models;

public class PatientServiceModel : IMapFrom<Patient>, IMapExplicitly
{
    public string UserId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string ImageUrl { get; set; }

    public string CityName { get; set; }

    public string Phone { get; set; }

    public string UserUserName { get; set; }

    public DateTime CreatedOn { get; set; }
    
    public void RegisterMappings(IProfileExpression profile)
    {
        profile.CreateMap<Patient, PatientServiceModel>()
            .ForMember(x => x.CreatedOn, opt =>
                opt.MapFrom(y => y.CreatedOn));
    }
}