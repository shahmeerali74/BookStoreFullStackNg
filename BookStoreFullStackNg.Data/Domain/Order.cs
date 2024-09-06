using BookStoreFullStackNg.Data.Constants;

namespace BookStoreFullStackNg.Data.Domain;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public Payment Payment { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = [];
}
