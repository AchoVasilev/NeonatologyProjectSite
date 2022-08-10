namespace Neonatology.ViewModels.Administration.Gallery;

using System;
using AutoMapper;
using Common.Mapping;
using Data.Models;

public class GalleryViewModel : IMapFrom<Image>, IMapExplicitly
{
    public string Id { get; set; }

    public string Url { get; set; }

    public DateTime CreatedOn { get; set; }
    
    public void RegisterMappings(IProfileExpression profile)
    {
        profile.CreateMap<Image, GalleryViewModel>()
            .ForMember(x => x.CreatedOn, opt =>
                opt.MapFrom(y => y.CreatedOn));
    }
}