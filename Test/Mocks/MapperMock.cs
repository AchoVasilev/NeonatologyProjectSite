﻿namespace Test.Mocks;

using AutoMapper;
using Neonatology.Web.MappingProfile;

public static class MapperMock
{
    public static IMapper Instance
    {
        get
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<MappingProfile>();
            });

            return new Mapper(mapperConfiguration);
        }
    }
}