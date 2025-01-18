namespace VideoProjector.DTOs.ShoppingCart
{
    public class ShoppingCartDto
    {
        public int ShoppingCartId { get; set; }
        public string CustomerId { get; set; } // Customer's ID
        public DateTime CreatedDate { get; set; }
        public List<ShoppingCartItemDto> Items { get; set; } // List of cart items
    }

}
