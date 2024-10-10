using AutoMapper;
using BookStoreFullStackNg.Data.Domain;
using BookStoreFullStackNg.Data.DTOs.Cart;
using BookStoreFullStackNg.Data.Reopositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreFullStackNg.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartRepository _cartRepo;
        private readonly IMapper _mapper;
        public CartsController(ICartRepository cartRepo, IMapper mapper)
        {
            _cartRepo = cartRepo;
            _mapper = mapper;
        }

        //[HttpPost]
        //public async Task<IActionResult> AddCartItem(CartItemCreateDto cartItemToCreate)
        //{
        //    var cartItem = _mapper.Map<CartItem>(cartItemToCreate);
        //    await _cartRepo.AddCartItemAsync(cartItem);
        //}
    }
}
