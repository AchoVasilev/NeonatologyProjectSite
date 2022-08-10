namespace Neonatology.ViewModels.Chat;

using System.Collections.Generic;
using AutoMapper;
using Common.Mapping;
using Common.Models;
using Data.Models;

public class ChatUserViewModel : PagingModel, IMapFrom<ApplicationUser>, IMapExplicitly
{
    public string Id { get; set; }

    public string FullName { get; set; }

    public string DoctorEmail { get; set; }

    public string GroupName { get; set; }

    public bool HasPaid { get; set; }

    public IEnumerable<ChatConversationsViewModel> ChatModels { get; set; }
    
    public void RegisterMappings(IProfileExpression profile)
    {
        profile.CreateMap<ApplicationUser, ChatUserViewModel>()
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
    }
}