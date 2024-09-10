using System.Net;
using System.Text;
using System.Text.Json;
using BookStoreFullStackNg.Data.Domain;
using BookStoreFullStackNg.Data.DTOs;

namespace BookStoreFullStackNg.IntegratedTests;

public class GenreControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{

    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private string baseUrl;

    public GenreControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        baseUrl = "api/genres";
    }

    [Fact]
    public async Task GetGenres_Returns_OkResponse()
    {
        // Arrange
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRolesHeader, "Admin");

        // Act
        var response = await _client.GetAsync(baseUrl);

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var genres = JsonSerializer.Deserialize<IEnumerable<GenreReadDto>>(responseString, options);

        // Assert
        Assert.NotNull(genres);
        Assert.NotEmpty(genres);
    }

    [Fact]
    public async Task GetGenreById_ReturnsOk_WhenGenreExists()
    {
        // Arrange
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRolesHeader, "Admin");
        var genreId = 1;

        // Act
        var response = await _client.GetAsync(baseUrl + "/" + genreId);

        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var genre = JsonSerializer.Deserialize<GenreReadDto>(responseString, options);

        // Assert
        Assert.NotNull(genre);
        Assert.Equal(genre.Id, genreId);
    }

    [Fact]
    public async Task GenGenreById_ReturnNotFound_WhenPersonDoesNotExist()
    {
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRolesHeader, "Admin");
        // Arrange
        int id = 999;

        // Act
        var response = await _client.GetAsync(baseUrl + "/" + id);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AddGenre_ReturnsCreatedAtRoute()
    {
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRolesHeader, "Admin");
        // arrange
        var newGenre = new GenreCreateDto { GenreName = "G1" };
        var content = new StringContent(JsonSerializer.Serialize(newGenre), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync(baseUrl, content);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var createdGenre = JsonSerializer.Deserialize<Genre>(jsonResponse, options);

        // Assert
        Assert.NotNull(createdGenre);
        Assert.Equal(newGenre.GenreName, createdGenre.GenreName);
    }

    [Fact]
    public async Task UpdateGenre_ReturnsNotContent_WhenSuccessfull()
    {
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRolesHeader, "Admin");
        // arrange
        var genreToUpdate = new GenreUpdateDto { Id = 1, GenreName = "G1" };
        var content = new StringContent(JsonSerializer.Serialize(genreToUpdate), Encoding.UTF8, "application/json");

        // act
        var response = await _client.PutAsync($"{baseUrl}/1", content);
        response.EnsureSuccessStatusCode();

        // assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateGenre_ReturnsBadRequest_WhenIdMismatch()
    {
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRolesHeader, "Admin");
        // arrange
        var genre = new GenreUpdateDto { Id = 1, GenreName = "G1" };
        var content = new StringContent(JsonSerializer.Serialize(genre), Encoding.UTF8, "application/json");

        // act
        var response = await _client.PutAsync($"{baseUrl}/2", content);

        // assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateGenre_ReturnsNotFound_WhenGenreDoesNotExists()
    {
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRolesHeader, "Admin");
        // arrange
        var genre = new GenreUpdateDto { Id = 999, GenreName = "G1" };
        var content = new StringContent(JsonSerializer.Serialize(genre), Encoding.UTF8, "application/json");

        // act
        var response = await _client.PutAsync($"{baseUrl}/999", content);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteGenre_ReturnsNoContent_WhenDeletionIsSuccessfull()
    {
        // arrange
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRolesHeader, "Admin");

        // act
        var response = await _client.DeleteAsync($"{baseUrl}/1");

        // assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteGenre_ReturnsNotFound_WhenPersonDoesNotExist()
    {
        // arrange
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRolesHeader, "Admin");

        // act
        var response = await _client.DeleteAsync($"{baseUrl}/9999");

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
