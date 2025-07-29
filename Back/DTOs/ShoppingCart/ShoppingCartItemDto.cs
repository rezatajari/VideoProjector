using System.ComponentModel.DataAnnotations;

namespace VideoProjector.DTOs.ShoppingCart
{
    public class ShoppingCartItemDto
    {
        [Required(ErrorMessage = "Shopping cart item ID is required.")]
        public int ShoppingCartItemId { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [MaxLength(100, ErrorMessage = "Product name cannot be longer than 100 characters.")]
        public string ProductName { get; set; } // You can add more product-related info, like name or image

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Total price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total price must be greater than zero.")]
        public decimal TotalPrice => Quantity * Price; // Calculated total price for the item
    }
}
