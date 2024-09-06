using AutoMapper;
using BookStoreFullStackNg.Api.Controllers;
using BookStoreFullStackNg.Data.Domain;
using BookStoreFullStackNg.Data.DTOs;
using BookStoreFullStackNg.Data.Reopositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace BookStoreFullStackNg.UnitTests;

public class GenreRepositoryTests
{

    private readonly IGenreRepository _genreRepository;
    private readonly IMapper _mapper;
    private readonly GenreController _genreController;

    static List<Genre> Genres = new List<Genre>
    {
       new Genre{Id=1, GenreName="Horror"},
       new Genre{Id=2, GenreName="Action"},
    };

    public GenreRepositoryTests()
    {
        _genreRepository = Substitute.For<IGenreRepository>();
        _mapper = Substitute.For<IMapper>();
        _genreController = new GenreController(_mapper, _genreRepository);
    }

    [Fact]
    public async Task GetGenres_ReturnsOk_With_GenreList()
    {
        // Arrange
        _genreRepository.GetGenres().Returns(Genres);
        _mapper.Map<IEnumerable<GenreReadDto>>(Arg.Any<List<Genre>>())
        .Returns(Genres.Select(g => new GenreReadDto { Id = g.Id, GenreName = g.GenreName }).ToList());

        // Act
        var result = await _genreController.GetGenres();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var genreList = Assert.IsType<List<GenreReadDto>>(okResult.Value);
        Assert.True(genreList.Count > 0);
    }
}