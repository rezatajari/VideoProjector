namespace VideoProjector.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; } // Primary Key
        public int ShoppingCartId { get; set; } // Foreign Key to ShoppingCart
        public int ProductId { get; set; } // Foreign Key to Product
        public int Quantity { get; set; } // Quantity of the product
        public decimal Price { get; set; } // Current price of the product

        // Navigation Properties
        public virtual ShoppingCart? ShoppingCart { get; set; } // Related shopping cart
        public virtual Product? Product { get; set; } // Related product
    }

}
