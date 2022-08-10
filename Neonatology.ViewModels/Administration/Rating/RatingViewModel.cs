namespace Neonatology.ViewModels.Administration.Rating;

using System;
using AutoMapper;
using Common.Mapping;
using Data.Models;

public class RatingViewModel : IMapFrom<Rating>, IMapExplicitly
{
    public int Id { get; init; }

    public DateTime CreatedOn { get; set; }

    public bool? IsConfirmed { get; set; } = false;

    public int Number { get; set; }

    public string Comment { get; set; }

    public int AppointmentId { get; set; }
    
    public void RegisterMappings(IProfileExpression profile)
    {
        profile.CreateMap<Rating, RatingViewModel>()
            .ForMember(x => x.CreatedOn, opt =>
                opt.MapFrom(y => y.CreatedOn.ToLocalTime()));
    }
}