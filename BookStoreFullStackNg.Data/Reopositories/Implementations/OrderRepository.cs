using BookStoreFullStackNg.Data.Constants;
using BookStoreFullStackNg.Data.Data;
using BookStoreFullStackNg.Data.Domain;
using BookStoreFullStackNg.Data.DTOs.Order;
using BookStoreFullStackNg.Data.Reopositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookStoreFullStackNg.Data.Reopositories.Implementations;

public class OrderRepository : IOrderRepository
{
    private readonly BookStoreContext _context;
    public OrderRepository(BookStoreContext context)
    {
        _context = context;
    }
    public async Task CreateOrder(int userId,OrderCreateDto orderToCreate)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var order = new Order
            {
                Name = orderToCreate.Name,
                Email = orderToCreate.Email,
                MobileNumber = orderToCreate.MobileNumber,
                OrderDate = DateTime.UtcNow,
                TaxInPercent=18,
                OrderStatus= OrderStatus.Pending,
                UserId=userId,
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // find the cart items of the user
            var cart = await _context.Carts.Include(c=>c.CartItems).FirstOrDefaultAsync(c=>c.UserId==userId);

            if(cart==null)
            {
                throw new InvalidOperationException("Cart is null");
            }

            var cartItems= await _context.CartItems.Where(ci=>ci.CartId==cart.Id).ToListAsync();

            if(cartItems.Count==0)
            {
                throw new InvalidOperationException("No cart items in the cart");
            }
            double subTotal = 0;
            // add cart items to the orderItem
            foreach (var cartItem in cart.CartItems)
            {
                var book = await _context.Books.AsNoTracking().SingleAsync(b => b.Id == cartItem.BookId);
          
                if(book==null)
                {
                    throw new InvalidOperationException("Book is null");
                }
          
                var orderItem = new OrderItem
                {
                   OrderId=order.Id,
                   BookId=cartItem.BookId,
                   Quantity=cartItem.Quantity,
                   Price=book.Price   
                };
                _context.OrderItems.Add(orderItem);
                subTotal += orderItem.Price * orderItem.Quantity;
            }
            await _context.SaveChangesAsync();

            // payment
            var total = subTotal* (order.TaxInPercent/ 100);
            var payment = new Payment
            {
                OrderId=order.Id,
                Amount = total,
                Status = PaymentStatus.Pending,
                Method = (PaymentMethod)orderToCreate.PaymentMethod
            };
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // delete cart items
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            // delete cart
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
