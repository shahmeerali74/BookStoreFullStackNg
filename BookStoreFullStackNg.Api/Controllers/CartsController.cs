using AutoMapper;
using BookStoreFullStackNg.Api.Exceptions;
using BookStoreFullStackNg.Data.Constants;
using BookStoreFullStackNg.Data.Domain;
using BookStoreFullStackNg.Data.DTOs.Cart;
using BookStoreFullStackNg.Data.Reopositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreFullStackNg.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class CartsController : ControllerBase
{
    private readonly ICartRepository _cartRepo;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepo;
    public CartsController(ICartRepository cartRepo, IMapper mapper, IUserRepository userRepo)
    {
        _cartRepo = cartRepo;
        _mapper = mapper;
        _userRepo = userRepo;
    }

    [HttpPost]
    public async Task<IActionResult> AddCartItem(CartItemCreateDto cartItemToCreate)
    {
        var userName = User.Identity.Name;
        if(userName==null)
        {
            throw new BadRequestException("User is not logged in");
        }
        var user = await _userRepo.GetUserByUserNameAsync(userName);

        // since username is not being passed by user, so this condition will likely occur
        if(user==null)
        {
            throw new BadRequestException("Invalid username");
        }
        var cartItem = _mapper.Map<CartItem>(cartItemToCreate);
        var createdCartItem=await _cartRepo.AddCartItemAsync(user.Id,cartItem);
        return CreatedAtAction(nameof(AddCartItem),createdCartItem);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCartItem(int cartItemId,CartItemUpdateDto cartItemToUpdate)
    {
        var userName = User.Identity.Name;
        if (userName == null)
        {
            throw new BadRequestException("User is not logged in");
        }
        var user = await _userRepo.GetUserByUserNameAsync(userName);

        // since username is not being passed by user, so this condition will likely occur
        if (user == null)
        {
            throw new BadRequestException("Invalid username");
        }

        if(cartItemId!=cartItemToUpdate.Id)
        {
            throw new BadHttpRequestException("Ids mismatch");
        }

        var existingCartItem = _cartRepo.GetCartItemByCartItemIdAsync(cartItemId);
        if (existingCartItem == null)
        {
            throw new BadRequestException($"Cart item {cartItemId} does not found");
        }

        var cartItem = _mapper.Map<CartItem>(cartItemToUpdate);
        var updatedCartItem = await _cartRepo.UpdateCartItemAsync(user.Id, cartItem);
        return Ok(updatedCartItem);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCartItem(int cartItemId)
    {
        var userName = User.Identity.Name;
        if (userName == null)
        {
            throw new BadRequestException("User is not logged in");
        }
        var user = await _userRepo.GetUserByUserNameAsync(userName);

        // since username is not being passed by user, so this condition will likely occur
        if (user == null)
        {
            throw new BadRequestException("Invalid username");
        }
        await _cartRepo.RemoveCartItemAsync(user.Id,cartItemId);
        return NoContent();
    }

    [HttpGet("UserCart")]
    public async Task<IActionResult> GetUserCartByUserId()
    {
        var userName = User.Identity.Name;
        if (userName == null)
        {
            throw new BadRequestException("User is not logged in");
        }
        var user = await _userRepo.GetUserByUserNameAsync(userName);

        // since username is not being passed by user, so this condition will likely occur
        if (user == null)
        {
            throw new BadRequestException("Invalid username");
        }
        var cart=await _cartRepo.GetCartByUserIdAsync(user.Id);
        return Ok(cart);
    }

    [HttpGet]
    //[Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetAllCarts()
    {
        var carts = await _cartRepo.GetCartsAsync();
        return Ok(carts);
    }
}
