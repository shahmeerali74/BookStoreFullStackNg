namespace BookStoreFullStackNg.Data.DTOs.Cart;

public class CartItemUpdateDto
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int Quantity { get; set; }
    public int BookId { get; set; }
}
