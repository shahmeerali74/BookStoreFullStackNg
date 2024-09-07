using System.Text.Json;
using BookStoreFullStackNg.Data.DTOs;

namespace BookStoreFullStackNg.IntegratedTests;

public class GenreControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{

    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public GenreControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetGenres_Returns_OkResponse()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("api/genres");

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var genres = JsonSerializer.Deserialize<IEnumerable<GenreReadDto>>(responseString, options);

        // Assert
        Assert.NotNull(genres);
        Assert.NotEmpty(genres);
    }
}
