using System;
using AutoMapper;
using BookStoreFullStackNg.Data.Domain;
using BookStoreFullStackNg.Data.DTOs;

namespace BookStoreFullStackNg.Api.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Genre, GenreCreateDto>().ReverseMap();
        CreateMap<Genre, GenreUpdateDto>().ReverseMap();
        CreateMap<Genre, GenreReadDto>().ReverseMap();
    }
}
