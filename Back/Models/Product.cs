namespace VideoProjector.Models
{
    public class Product
    {
        public int ProductId { get; set; }  // Primary Key
        public string? Name { get; set; } // Product name (e.g., "Video Projector")
        public string? Description { get; set; } // Description of the product
        public decimal Price { get; set; }  // Price of the product
        public int StockQuantity { get; set; }  // Number of items available in stock
        public string? ImageUrl { get; set; }  // URL of the product image (optional)

        // Foreign Key to Category (if you want to associate products with categories)
        public int CategoryId { get; set; }
        // Navigation property to Category (many products can belong to one category)
        public virtual Category? Category { get; set; }

        // Navigation property to OrderDetails (many order details can link to one product)
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }

}
