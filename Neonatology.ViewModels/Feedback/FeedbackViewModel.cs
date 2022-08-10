namespace Neonatology.ViewModels.Feedback;

using AutoMapper;
using Common.Mapping;
using Data.Models;
using static Common.Constants.GlobalConstants.DateTimeFormats;
public class FeedbackViewModel : IMapFrom<Feedback>, IMapExplicitly
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Type { get; set; }

    public bool IsSolved { get; set; }

    public string Comment { get; set; }

    public string CreatedOn { get; set; }
    
    public void RegisterMappings(IProfileExpression profile)
    {
        profile.CreateMap<Feedback, FeedbackViewModel>()
            .ForMember(x => x.CreatedOn, opt =>
                opt.MapFrom(x => x.CreatedOn.ToString(DateTimeFormat)));
    }
}