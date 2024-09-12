using System;
using AutoMapper;
using BookStoreFullStackNg.Data.Domain;
using BookStoreFullStackNg.Data.DTOs;
using BookStoreFullStackNg.Data.DTOs.Author;

namespace BookStoreFullStackNg.Api.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Genre, GenreCreateDto>().ReverseMap();
        CreateMap<Genre, GenreUpdateDto>().ReverseMap();
        CreateMap<Genre, GenreReadDto>().ReverseMap();
        CreateMap<Author, AuthorCreateDTO>().ReverseMap();
        CreateMap<Author, AuthorUpdateDTO>().ReverseMap();
        CreateMap<Author, AuthorReadDTO>().ReverseMap();
    }
}
