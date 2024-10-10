using BookStoreFullStackNg.Data.Data;
using BookStoreFullStackNg.Data.Domain;
using BookStoreFullStackNg.Data.Reopositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStoreFullStackNg.Data.Reopositories.Implementations;

public class CartRepository : ICartRepository
{
    private readonly BookStoreContext _context;

    public CartRepository(BookStoreContext context)
    {
        _context = context;
    }
    public async Task AddCartItemAsync(int userId,CartItem cartItem)
    {
        using var tran= await _context.Database.BeginTransactionAsync();
        try
        {
            var cart = await GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            // if item is already in cart,then increment else add
            var existingItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.BookId == cartItem.BookId);
            if (existingItem == null)
            {
                 cartItem.CartId = cart.Id;
                _context.CartItems.Add(cartItem);
            }
            else
            {
                existingItem.Quantity += cartItem.Quantity;
            }
            await _context.SaveChangesAsync();
            await tran.CommitAsync();
        }
        catch(Exception)
        {
           await tran.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateCartItemAsync(int userId,CartItem cartItem)
    {
        var cart = await GetCartByUserIdAsync(userId);
        if (cart == null)
        {
            throw new InvalidOperationException("This user does not have any items in cart");
        }
        //var existingCartItem = await _context.CartItems.AsNoTracking().FirstOrDefaultAsync(ci => ci.Id == cartItem.Id);
        //if (existingCartItem == null)
        //{
        //    throw new InvalidOperationException("Cart item is null");
        //}
        if (cartItem.CartId != cart.Id)
        {
            throw new InvalidOperationException("You are updating the wrong cart");
        }
        _context.CartItems.Update(cartItem);
        await _context.SaveChangesAsync();
    }

    public async Task<Cart?> GetCartByIdAsync(int cartId)
    {
        return await _context.Carts.Include(c=>c.CartItems).ThenInclude(ci=>ci.Book).AsNoTracking().FirstOrDefaultAsync(c => c.Id == cartId);
    }

    public async Task<Cart?> GetCartByUserIdAsync(int userId)
    {
        return await _context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.Book).AsNoTracking().FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<CartItem?> GetCartItemByCartItemIdAsync(int cartItemId)
    {
        return await _context.CartItems.Include(ci => ci.Book).AsNoTracking().FirstOrDefaultAsync(ci=>ci.Id==cartItemId);
    }

    public async Task RemoveCartItemAsync(int userId,int cartItemId)
    {
        var cart = await GetCartByUserIdAsync(userId);
        if (cart == null)
        {
            throw new InvalidOperationException("This user does not have any items in cart");
        }
        var cartItem= await _context.CartItems.AsNoTracking().FirstOrDefaultAsync(ci=>ci.Id==cartItemId);
        if(cartItem == null)
        {
            throw new InvalidOperationException("Cart item is null");
        }
        if(cartItem.CartId!=cart.Id)
        {
            throw new InvalidOperationException("You are deleting the item of a wrong cart");
        }
        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();
    }
}
