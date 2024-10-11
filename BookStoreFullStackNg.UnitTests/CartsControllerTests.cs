using AutoMapper;
using BookStoreFullStackNg.Api.Controllers;
using BookStoreFullStackNg.Data.Domain;
using BookStoreFullStackNg.Data.DTOs.Cart;
using BookStoreFullStackNg.Data.Reopositories.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Security.Claims;
using System.Security.Principal;

namespace BookStoreFullStackNg.UnitTests;

/// <summary>
/// For learning purpose, I've used fluent assertions library in these tests, which is not used in previous tests.
/// </summary>
public class CartsControllerTests
{
    private readonly ICartRepository _cartRepo;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepo;
    private readonly CartsController _controller;
    //private readonly HttpContext _httpContext;

    public CartsControllerTests()
    {
        _cartRepo = Substitute.For<ICartRepository>();
        _mapper = Substitute.For<IMapper>();
        _userRepo = Substitute.For<IUserRepository>();
        //_httpContext = Substitute.For<HttpContext>();
        _controller = new CartsController(_cartRepo, _mapper, _userRepo);
    }

    //[Fact]
    //public async Task AddCartItem_ReturnCreateAtAction_WhenUserIsLoggedIn()
    //{
    //    // arrange
    //    var cartItemToCreate = new CartItemCreateDto
    //    {
    //        BookId = 1,
    //        Quantity = 1,
    //    };
    //    var userName = "testUser";
    //    var userMock = new User { Id = 1, Username = userName, Name = "Test Name" };
    //    var cartItemMock= new CartItem { Id=1,BookId=1,Quantity=1};
    //    var createdCartItemMock= new CartItem { Id = 1, BookId = 1, Quantity = 1 };
    //    var cartItemDtoMock = new CartItemDto { Id=1,BookId=1,Quantity=1};

    //    // mocking User.Identity.Name

    //    var identity = new GenericIdentity(userName, "test");
    //    var contextUser = new ClaimsPrincipal(identity);
    //    _controller.ControllerContext = new ControllerContext
    //    {
    //        HttpContext = new DefaultHttpContext { User = contextUser }
    //    };


    //    // mocking repository and mapper behavior
    //    _userRepo.GetUserByUserNameAsync(userName).Returns(userMock);
    //    _mapper.Map<CartItem>(cartItemToCreate).Returns(cartItemMock);
    //    _cartRepo.AddCartItemAsync(userMock.Id, cartItemMock).Returns(createdCartItemMock);
    //    _mapper.Map<CartItemDto>(createdCartItemMock).Returns(cartItemDtoMock);

    //    // act
    //    var result = await _controller.AddCartItem(cartItemToCreate);

    //    // assert
    //    var createdAtActionResult= result.Should().BeOfType<CreatedAtActionResult>().Subject;
    //    Assert.Equal(nameof(_controller.AddCartItem), createdAtActionResult.ActionName);
    //    createdAtActionResult.ActionName.Should().Be(nameof(_controller.AddCartItem));
    //    var returnedItem = createdAtActionResult.Value.Should().BeOfType<CartItemDto>().Subject;
    //    returnedItem.Id.Should().Be(cartItemDtoMock.Id);
    //}
}
