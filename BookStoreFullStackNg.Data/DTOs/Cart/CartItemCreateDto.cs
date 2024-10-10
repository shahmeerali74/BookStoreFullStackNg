namespace BookStoreFullStackNg.Data.DTOs.Cart;

public class CartItemCreateDto
{
    public int CartId { get; set; }
    public int Quantity { get; set; }
    public int BookId { get; set; }
}
