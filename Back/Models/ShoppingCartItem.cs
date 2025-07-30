namespace VideoProjector.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; } 
        public int ShoppingCartId { get; set; } 
        public int ProductId { get; set; } 
        public int Quantity { get; set; } 
        public decimal Price { get; set; }

        public virtual ShoppingCart? ShoppingCart { get; set; } 
        public virtual Product? Product { get; set; } 
    }

}
