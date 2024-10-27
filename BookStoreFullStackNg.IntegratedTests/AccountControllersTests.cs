using BookStoreFullStackNg.Data.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace BookStoreFullStackNg.IntegratedTests;

[Collection("Test Collection")]
public class AccountControllersTests
{
    private readonly CustomWebApplicationFactory _webApplicationFactory;
    private readonly HttpClient _client;
    private string _url = "api/account";

    public AccountControllersTests(CustomWebApplicationFactory webApplicationFactory)
    {
        _webApplicationFactory = webApplicationFactory;
        _client = webApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task Signup_ReturnsCreatedAtAction_OnSuccess()
    {
        // Arrange
        var registrationModel = new RegistrationModel { Name = "John", Email = "john@gmail.com", Password = "John@123" };

        // Act
        var response = await _client.PostAsJsonAsync<RegistrationModel>(_url+"/signup",registrationModel);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdItem = await response.Content.ReadFromJsonAsync<RegistrationModel>();
        Assert.Equal(createdItem!.Name, registrationModel.Name);
    }

}
