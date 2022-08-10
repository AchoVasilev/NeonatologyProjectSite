namespace Neonatology.ViewModels.Chat;

using AutoMapper;
using Common.Mapping;
using Data.Models;

public class ChatConversationsViewModel : IMapFrom<ApplicationUser>, IMapExplicitly
{
    public string Id { get; set; }

    public string FullName { get; set; }

    public string UserName { get; set; }

    public string UserImageUrl { get; set; }

    public string LastMessage { get; set; }

    public string LastMessageActivity { get; set; }

    public string GroupName { get; set; }

    public void RegisterMappings(IProfileExpression profile)
    {
        profile.CreateMap<ApplicationUser, ChatConversationsViewModel>()
            .ForMember(x => x.FullName,
                opt =>
                {
                    opt.MapFrom(y =>
                        y.Doctor.FirstName != null
                            ? "Д-р " + y.Doctor.FirstName + " " + y.Doctor.LastName
                            : y.Patient.FirstName + " " + y.Patient.LastName);
                })
            .ForMember(x => x.UserImageUrl, opt =>
            {
                opt.MapFrom(y => y.Image.Url);
            });
    }
}