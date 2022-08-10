namespace Neonatology.ViewModels.Chat;

using AutoMapper;
using Common.Constants;
using Common.Mapping;
using Data.Models;

public class ChatMessageWithUserViewModel : IMapFrom<Message>, IMapExplicitly
{
    public string Content { get; set; }

    public string SenderId { get; set; }

    public string FullName { get; set; }

    public string CreatedOn { get; set; }

    public bool CanChat { get; set; }
    
    public void RegisterMappings(IProfileExpression profile)
    {
        profile.CreateMap<Message, ChatMessageWithUserViewModel>()
            .ForMember(vm => vm.CreatedOn, opt =>
                opt.MapFrom(m => m.CreatedOn.AddHours(3).ToString(GlobalConstants.DateTimeFormats.DateTimeFormat)))
            .ForMember(vm => vm.FullName, opt =>
                opt.MapFrom(m => m.Sender.Doctor.FirstName != null
                    ? "Д-р " + m.Sender.Doctor.FirstName + " " + m.Sender.Doctor.LastName
                    : m.Sender.Patient.FirstName + " " + m.Sender.Patient.LastName));
    }
}