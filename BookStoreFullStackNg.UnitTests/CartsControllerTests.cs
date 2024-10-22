using AutoMapper;
using BookStoreFullStackNg.Api.Controllers;
using BookStoreFullStackNg.Api.Exceptions;
using BookStoreFullStackNg.Data.Domain;
using BookStoreFullStackNg.Data.DTOs.Cart;
using BookStoreFullStackNg.Data.Reopositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Security.Claims;
using BookStoreFullStackNg.Api.Helpers.Wrapper;
using NSubstitute.ExceptionExtensions;


namespace BookStoreFullStackNg.UnitTests;

public class CartsControllerTests
{
    private readonly ICartRepository _cartRepo;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepo;
    private readonly CartsController _controller;
    private readonly ICartItemMapper _cartItemMapper;
    public CartsControllerTests()
    {
        _cartRepo = Substitute.For<ICartRepository>();
        _mapper = Substitute.For<IMapper>();
        _userRepo = Substitute.For<IUserRepository>();
        _cartItemMapper = Substitute.For<ICartItemMapper>();
        _controller = new CartsController(_cartRepo, _mapper, _userRepo,_cartItemMapper);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "testuser")
        }));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task AddCartItem_UserNotLoggedIn_ThrowsBadRequestException()
    {
        // Arrange
        _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity()); // No Name claim

        var cartItemDto = new CartItemCreateDto(); // Adjust according to your DTO structure

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(async () =>
            await _controller.AddCartItem(cartItemDto));
    }

    [Fact]
    public async Task AddCartItem_UserNotFound_ThrowsBadRequestException()
    {
        // Arrange
        var cartItemDto = new CartItemCreateDto(); // Adjust according to your DTO structure

        _userRepo.GetUserByUserNameAsync(Arg.Any<string>()).Returns(Task.FromResult<User>(null));

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(async () =>
            await _controller.AddCartItem(cartItemDto));
    }


    [Fact]
    public async Task AddCartItem_ValidRequest_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var userName = "testuser";
        var cartItemDto = new CartItemCreateDto(); // Adjust according to your DTO structure
        var user = new User { Id = 1, Username = userName };  // Mock user entity
        var cartItem = new CartItem(); // Adjust according to your entity structure

        // Mock HttpContext and set user identity
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, userName)
        }))
        };

        var createdCartItem = new CartItem
        {
            Id = 1,
            BookId = 123,
            Quantity = 2
        };

        var cartItemToReturn = new CartItemDto
        {
            Id = 1,
            BookId = 123,
            Quantity = 2
        };

        // Set up repository and mapper behavior
        _userRepo.GetUserByUserNameAsync(userName).Returns(user);
        _mapper.Map<CartItem>(cartItemDto).Returns(cartItem);
        _cartRepo.AddCartItemAsync(user.Id, cartItem).Returns(createdCartItem);

        // Mock the wrapper behavior
        var cartItemMapper = Substitute.For<ICartItemMapper>();
        cartItemMapper.MapCartItemToCartItemDto(createdCartItem).Returns(cartItemToReturn);

        // Create controller and assign HttpContext
        var controller = new CartsController(_cartRepo, _mapper, _userRepo, cartItemMapper)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = httpContext // Set the mocked HttpContext
            }
        };

        // Act
        var result = await controller.AddCartItem(cartItemDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(CartsController.AddCartItem), createdResult.ActionName);
        Assert.Equal(cartItemToReturn, createdResult.Value);
    }

    [Fact]
    public async Task UpdateCartItem_ValidRequest_ReturnsOk()
    {
        // Arrange
        string username = "testuser";
        int cartItemId = 1;

        var cartItemToUpdate = new CartItemUpdateDto
        {
         Id=cartItemId,
         BookId=123,
         Quantity=3
        };
        var user = new User {Id=1,Username=username };
        var existingCartItem = new CartItem { Id = cartItemId, BookId = 123, Quantity = 2 };
        var updatedCartItem = new CartItem{
            Id = cartItemId,
         BookId = 123,
         Quantity = 3
        };
        var cartItemDto = new CartItemDto { Id = cartItemId, BookId = 123, Quantity = 3 }; // Mock cart item DTO for return

        // Mock HttpContext and set user identity
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, username)
        }))
        };

        _controller.ControllerContext.HttpContext = httpContext;

        // Set up repository and mapper behavior
        _userRepo.GetUserByUserNameAsync(username).Returns(user);
        _cartRepo.GetCartItemByCartItemIdAsync(cartItemId).Returns(existingCartItem);
        _mapper.Map<CartItem>(cartItemToUpdate).Returns(updatedCartItem);
        _cartRepo.UpdateCartItemAsync(user.Id, updatedCartItem).Returns(updatedCartItem);

        // mock the cart item mapper
        _cartItemMapper.MapCartItemToCartItemDto(updatedCartItem).Returns(cartItemDto);

        // act
        var result = await _controller.UpdateCartItem(cartItemToUpdate.Id, cartItemToUpdate);

        // assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCartItem = Assert.IsType<CartItemDto>(okResult.Value);
        Assert.Equal(cartItemDto.Id, returnedCartItem.Id);
        Assert.Equal(cartItemDto.BookId, returnedCartItem.BookId);
        Assert.Equal(cartItemDto.Quantity, returnedCartItem.Quantity);
    }

    [Fact]
    public async Task UpdateCartItem_UserNotAuthenticated_ThrowsBadRequestException()
    {
        // arrange
        var cartItemToUpdate = new CartItemUpdateDto { Id = 1, BookId = 123, Quantity = 2 };
        _controller.ControllerContext.HttpContext = new DefaultHttpContext()
        {
            User = new ClaimsPrincipal(new ClaimsIdentity()) // No identity
        };

        // Act and assert
        await Assert.ThrowsAsync<BadRequestException>(async () => await _controller.UpdateCartItem(cartItemToUpdate.Id, cartItemToUpdate));

    }

    [Fact]
    public async Task UpdateCartItem_UserNotFound_ThrowsBadRequestException()
    {
        // arrange
        var username = "testUser";
        var cartItemToUpdate = new CartItemUpdateDto { Id = 1, BookId = 2, Quantity = 3 };
        _controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
         {
             new Claim(ClaimTypes.Name,username)
         }))
        };

        _userRepo.GetUserByUserNameAsync(username).Returns(Task.FromResult<User>(null));

        // act and assert
        await Assert.ThrowsAsync<BadRequestException>(async () => await _controller.UpdateCartItem(cartItemToUpdate.Id, cartItemToUpdate));
    }

    [Fact]
    public async Task UpdateCartItem_IdsMismatch_ThrowsBadHttpRequestException()
    {
        // Arrange
        var userName = "testuser";
        var cartItemId = 1;
        var cartItemToUpdate = new CartItemUpdateDto { Id = 2, BookId = 123, Quantity = 2 }; // Mismatched IDs

        _controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, userName)
        }))
        };

        var user = new User { Id = 1, Username = userName };
        _userRepo.GetUserByUserNameAsync(userName).Returns(user);

        // Act & Assert
        await Assert.ThrowsAsync<BadHttpRequestException>(async () =>
            await _controller.UpdateCartItem(cartItemId, cartItemToUpdate));
    }

    [Fact]
    public async Task UpdateCartItem_CartItemNotFound_ThrowsBadRequestException()
    {
        // Arrange
        var userName = "testuser";
        var cartItemId = 1;
        var cartItemToUpdate = new CartItemUpdateDto { Id = 1, BookId = 123, Quantity = 2 };

        _controller.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, userName)
        }))
        };

        var user = new User { Id = 1, Username = userName };
        _userRepo.GetUserByUserNameAsync(userName).Returns(user);

        _cartRepo.GetCartItemByCartItemIdAsync(cartItemId).Returns(Task.FromResult<CartItem?>(null)); // Cart item not found

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(async () =>
            await _controller.UpdateCartItem(cartItemId, cartItemToUpdate));
    }

    [Fact]
    public async Task UpdateCartItem_UpdateFails_ReturnsServerError() 
    {
        // Arrange
        string username = "testuser";
        var cartItemId = 1;
        var cartItemToUpdate = new CartItemUpdateDto { Id = cartItemId, BookId = 123, Quantity = 2 };

        _controller.ControllerContext.HttpContext = new DefaultHttpContext {
         User = new ClaimsPrincipal(new ClaimsIdentity(
               [new Claim(ClaimTypes.Name,username)]
             ))
        };

        // mocking user
        var mockUser = new User { Id = 1, Name = "test", Username = username };
        _userRepo.GetUserByUserNameAsync(username).Returns(mockUser);

        // mocking get cart item
        var existingCartItem = new CartItem { Id = cartItemToUpdate.Id, BookId=cartItemToUpdate.BookId, Quantity = cartItemToUpdate.Quantity };
        _cartRepo.GetCartItemByCartItemIdAsync(cartItemId).Returns(existingCartItem);

        //mocking mapper
        _mapper.Map<CartItem>(Arg.Any<CartItemUpdateDto>()).Returns(existingCartItem);
        //mocking updated cartItem

        _cartRepo.UpdateCartItemAsync(cartItemId, existingCartItem).ThrowsAsync(new Exception("Failed to update the cart item"));

        // Act and Assert
        var exception=await Assert.ThrowsAsync<Exception>(async () => await _controller.UpdateCartItem(cartItemId, cartItemToUpdate));
        Assert.Equal("Failed to update the cart item", exception.Message);

    }



}
