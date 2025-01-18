namespace VideoProjector.DTOs.ShoppingCart
{
    public class ShoppingCartItemDto
    {
        public int ShoppingCartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } // You can add more product-related info, like name or image
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Quantity * Price; // Calculated total price for the item
    }

}
