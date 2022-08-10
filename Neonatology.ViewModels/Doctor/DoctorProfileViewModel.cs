namespace Neonatology.ViewModels.Doctor;

using System.Collections.Generic;
using Address;
using AutoMapper;
using Common.Mapping;
using Data.Models;

public class DoctorProfileViewModel : IMapFrom<Doctor>, IMapExplicitly
{
    public string Id { get; set; }

    public string FullName { get; set; }

    public string UserImageUrl { get; set; }

    public int Age { get; set; }

    public string PhoneNumber { get; set; }

    public int? YearsOfExperience { get; set; }

    public string Email { get; set; }

    public string Biography { get; set; }

    public ICollection<AddressFormModel> Addresses { get; set; }

    public ICollection<SpecializationFormModel> Specializations { get; set; }
    
    public void RegisterMappings(IProfileExpression profile)
    {
        profile.CreateMap<Doctor, DoctorProfileViewModel>()
            .ForMember(x => x.UserImageUrl, opt =>
            {
                opt.MapFrom(d => d.User.Image.Url ?? d.User.Image.RemoteImageUrl);
            })
            .ForMember(x => x.FullName, opt =>
            {
                opt.MapFrom(d => string.Join(' ', d.FirstName, d.LastName));
            });
    }
}