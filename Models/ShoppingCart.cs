namespace VideoProjector.Models
{
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; } // Primary Key
        public string? CustomerId { get; set; }  // Foreign Key to Identity User
        public DateTime CreatedDate { get; set; } // Date when the cart was created.

        // Navigation Property
        public Customer? Customer { get; set; } // Navigation property for the Customer.
        public virtual ICollection<ShoppingCartItem>? Items { get; set; } // List of items in the cart

    }

}
