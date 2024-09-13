
using AutoMapper;
using BookStoreFullStackNg.Api.Controllers;
using BookStoreFullStackNg.Api.Exceptions;
using BookStoreFullStackNg.Data.Domain;
using BookStoreFullStackNg.Data.DTOs;
using BookStoreFullStackNg.Data.DTOs.Author;
using BookStoreFullStackNg.Data.Reopositories.Implementations;
using BookStoreFullStackNg.Data.Reopositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace BookStoreFullStackNg.UnitTests;

public class AuthorControllerTests
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;
    private readonly AuthorController _authorController;

    private static List<Author> authors = [
                                           new Author{Id=1,AuthorName="Author 1"},
                                           new Author{Id=2,AuthorName="Author 2"}
                                          ];

    public AuthorControllerTests()
    {
        _authorRepository = Substitute.For<IAuthorRepository>();
        _mapper = Substitute.For<IMapper>();
        _authorController = new AuthorController(_authorRepository, _mapper);
    }

    // Face
    // public async Task GetAuthors_ReturnsOk_WithAuthorsList()
    // {
    //     // Arrange
    //     _authorRepository.GetAuthors(new AuthorQueryParameters()).;
    // }

    [Fact]
    public async Task CreateAuthor_ReturnsCreatedAtActionResult_With_AuthorReadDto()
    {
        // Arrange
        AuthorCreateDTO authorToAdd = new() { AuthorName = "test" };
        Author expectedAuthor = new() { Id = 1, AuthorName = "test" };
        AuthorReadDTO authorReadDTO = new() { Id = 1, AuthorName = "test" };

        _mapper.Map<Author>(Arg.Any<AuthorCreateDTO>()).Returns(expectedAuthor);
        _authorRepository.CreateAuthor(Arg.Any<Author>()).Returns(expectedAuthor);
        _mapper.Map<AuthorReadDTO>(Arg.Any<Author>()).Returns(authorReadDTO);

        // Act
        var result = await _authorController.CreateAuthor(authorToAdd);

        // Assert
        var createdResult = Assert.IsType<CreatedAtRouteResult>(result);
        Assert.Equal(nameof(_authorController.GetAuthor), createdResult.RouteName);
        Assert.Equal(1, createdResult.RouteValues["id"]);

        var createdAuthor = Assert.IsType<AuthorReadDTO>(createdResult.Value);

        Assert.NotNull(createdAuthor);
        Assert.Equal(authorReadDTO.Id, createdAuthor.Id);

    }

    [Fact]
    public async Task GetAuthor_ReturnsOk_With_AuthorReadDto()
    {
        // Arrange
        var expectedAuthor = authors.First();
        _authorRepository.GetAuthor(Arg.Any<int>()).Returns(expectedAuthor);
        var authorReadDto = new AuthorReadDTO { Id = expectedAuthor.Id, AuthorName = expectedAuthor.AuthorName };
        _mapper.Map<AuthorReadDTO>(Arg.Any<Author>()).Returns(authorReadDto);

        // Act
        var result = await _authorController.GetAuthor(expectedAuthor.Id);

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var author = Assert.IsType<AuthorReadDTO>(okObjectResult.Value);
        Assert.NotNull(author);
        Assert.Equal(expectedAuthor.Id, author.Id);
        Assert.Equal(expectedAuthor.AuthorName, author.AuthorName);
    }

    [Fact]
    public async Task GetAuthor_ThrowsNotFoundFoundException_WhenAuthorDoesNotExist()
    {
        // Arrange
        int id = 9999;

        // Act and  Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _authorController.GetAuthor(id));
    }

    [Fact]
    public async Task DeleteAuthor_ReturnsNoContent_OnDeletion()
    {
        // Arrange
        var authorToDelete = authors.First();
        _authorRepository.GetAuthor(authorToDelete.Id).Returns(authorToDelete);

        // Act
        var result = await _authorController.DeleteAuthor(authorToDelete.Id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
