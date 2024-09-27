using BookStoreFullStackNg.Data.Constants;
using BookStoreFullStackNg.Data.DTOs.Book;
using BookStoreFullStackNg.Data.DTOs.Common;
using System.Net;
using System.Text;
using System.Text.Json;

namespace BookStoreFullStackNg.IntegratedTests;

public class BooksControllerTests: IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _webApplicationFactory;
    private readonly HttpClient _client;
    private string baseUrl = "api/books";

    public BooksControllerTests(CustomWebApplicationFactory<Program> webApplicationFactory)
    {
        _webApplicationFactory = webApplicationFactory;
        _client = webApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task GetBooks_Returns_OkResponse()
    {
        // arrange
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRolesHeader, Roles.Admin);

        // act
        var response = await _client.GetAsync(baseUrl);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var books = JsonSerializer.Deserialize<PagedList<BookReadDto>>(responseString, options);

        // assert
        Assert.NotNull(books);
        Assert.NotEmpty(books.Items);
    }

    [Fact]
    public async Task GetBookById_ReturnOk()
    {
        // arrange
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRolesHeader, Roles.Admin);

        // act
        var result = await _client.GetAsync(baseUrl+"/1");
        result.EnsureSuccessStatusCode();
        var stringResult = await result.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var book = JsonSerializer.Deserialize<BookReadDto>(stringResult, options);


        // assert
        Assert.NotNull(book);
    }

    [Fact]
    public async Task GetBookReturns_NotFound_WhenUserDoesNotExist()
    {
        // arrange
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRolesHeader, Roles.Admin);

        // act
        var response = await _client.GetAsync(baseUrl+"/999");

        // assert
        Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
    }

    [Fact]
    public async Task AddBook_Returns_CreatedAtRoute()
    {
        // arrange
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRolesHeader, Roles.Admin);
        var book = new BookCreateDto
        {
            Title = "book3",
            Description = "description3",
            Price = 124,
            ImageFile = null,
            PublishedYear = 2008,
            GenreIds = { 7 },
            AuthorIds = { 1 }
        };
        var jsonData = JsonSerializer.Serialize(book);
        var content = new StringContent(jsonData,Encoding.UTF8, "application/json");

        // act
        var response = await _client.PostAsync(baseUrl, content);
       // response.EnsureSuccessStatusCode();
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var createdBook = JsonSerializer.Deserialize<BookReadDto>(jsonResponse, options);

        // assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);    
        Assert.NotNull(createdBook);
    }

    [Fact]
    public async Task UpdateBook_ReturnsNoContent()
    {
        // arrange
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRolesHeader, Roles.Admin);
        var book=new BookUpdateDto
        {
            Id=1,
            Title = "book1",
            Description = "ddd",
            Price = 123,
            ImageFile = null,
            ImageUrl=null,
            PublishedYear = 2008,
            GenreIds = { 1 },
            AuthorIds = { 1 }
        };
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var jsonData = JsonSerializer.Serialize(book,options);
        var content = new StringContent(jsonData,Encoding.UTF8, "application/json");

        // act
        var response = await _client.PutAsync(baseUrl + "/1", content);
        var jsonResponse = await response.Content.ReadAsStringAsync();
        //response.EnsureSuccessStatusCode();

        // assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateBook_ReturnsBadRequest_WhenIdMismatch()
    {
        // arrange
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRolesHeader, Roles.Admin);
        var book = new BookUpdateDto
        {
            Id = 1,
            Title = "book1",
            Description = "ddd",
            Price = 123,
            ImageFile = null,
            PublishedYear = 2008,
            GenreIds = { 1 },
            AuthorIds = { 1 }
        };
        var jsonData= JsonSerializer.Serialize(book);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        // act
        var response = await _client.PutAsync(baseUrl + "/2", content);
        
        // assert
        Assert.Equal(HttpStatusCode.BadRequest,response.StatusCode);
    }

    [Fact]
    public async Task UpdateBook_ReturnsNotFound_WhenBookNotFound()
    {
        // arrange
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRolesHeader, Roles.Admin);
        var book = new BookUpdateDto
        {
            Id = 999,
            Title = "book1",
            Description = "ddd",
            Price = 123,
            ImageFile = null,
            PublishedYear = 2008,
            GenreIds = [ 1 ],
            AuthorIds = [ 1 ]
        };
        var jsonData = JsonSerializer.Serialize(book);
        var content = new StringContent(jsonData,Encoding.UTF8, "application/json");

        // act
        var response = await _client.PutAsync(baseUrl + "/999", content);

        // assert
        Assert.Equal(HttpStatusCode.NotFound,response.StatusCode);
    }

    [Fact]
    public async Task DeleteBook_ReturnsNoContent()
    {
        // arrange
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRolesHeader, Roles.Admin);

        // act
        var response = await _client.DeleteAsync(baseUrl + "/2");
        response.EnsureSuccessStatusCode();

        // assert
        Assert.Equal(HttpStatusCode.NoContent,response.StatusCode);
    }

    [Fact]
    public async Task DeleteBook_ReturnsNotFound_WhenBookDoesNotExists()
    {
        // arrange
        _client.DefaultRequestHeaders.Add(TestAuthHandler.TestUserRolesHeader, Roles.Admin);

        // act
        var response = await _client.DeleteAsync(baseUrl + "/999");

        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

}

