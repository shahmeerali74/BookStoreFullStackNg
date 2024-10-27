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
        var registrationModel = new RegistrationModel { Name = "Test", Email = "test@gmail.com", Password = "Test@123" };

        // Act
        var response = await _client.PostAsJsonAsync<RegistrationModel>(_url+"/signup",registrationModel);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdItem = await response.Content.ReadFromJsonAsync<RegistrationModel>();
        Assert.Equal(createdItem!.Name, registrationModel.Name);
    }

    [Fact]
    public async Task Login_ReturnToken_OnSuccess()
    {
        // Arrange
        var registrationModel = new RegistrationModel { Name = "John", Email = "jack@gmail.com", Password = "Jack@123" };
        await _client.PostAsJsonAsync<RegistrationModel>(_url + "/signup", registrationModel);
        var loginModel = new LoginModel { Username = "jack@gmail.com", Password="Jack@123" };

        // Act
        var response= await _client.PostAsJsonAsync($"{_url}/login", loginModel);

        // Assert
        Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        var token = await response.Content.ReadAsStringAsync();
        Assert.NotNull(token);
    }
}
