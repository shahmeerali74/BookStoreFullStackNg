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
        // arrange
        var genre = new GenreUpdateDto { Id = 999, GenreName = "G1" };
        var content = new StringContent(JsonSerializer.Serialize(genre), Encoding.UTF8, "application/json");

        // act
        var response = await _client.PutAsync($"{baseUrl}/999", content);

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
