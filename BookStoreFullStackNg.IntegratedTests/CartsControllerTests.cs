using BookStoreFullStackNg.Data.DTOs.Cart;
using System.Net;
using System.Text;
using System.Text.Json;

namespace BookStoreFullStackNg.IntegratedTests;

public class CartsControllerTests: IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private const string _url = "api/carts";
    public CartsControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task AddCartItem_Returns_CreatedAtAction()
    {
        // Arrange
        var cartItemToCreate = new CartItemCreateDto {BookId=1,Quantity=1 };
        var content = new StringContent(JsonSerializer.Serialize(cartItemToCreate),Encoding.UTF8,"application/json");

        // Act
        var response= await _client.PostAsync(_url,content);
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive=true };
        var createdItem =  JsonSerializer.Deserialize<CartItemDto>(jsonResponse, options);

        // Assert
        Assert.NotNull(createdItem);
        Assert.Equal(cartItemToCreate.BookId, createdItem.BookId);
    }

    [Fact]
    public async Task AddCartItem_Throws_UnAuthenticated_WhenUserNotLoggedIn()
    {
        // Arrange
        var cartItemToCreate = new CartItemCreateDto { BookId = 1, Quantity = 1 };
        var content = new StringContent(JsonSerializer.Serialize(cartItemToCreate), Encoding.UTF8, "application/json");

        // Act: Force unauthenticated request by adding this header
        _client.DefaultRequestHeaders.Add("X-ForceUnAuthenticated", "true");

        var response = await _client.PostAsync(_url, content);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }


}
