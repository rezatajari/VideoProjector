using System.ComponentModel.DataAnnotations;

namespace VideoProjector.DTOs.ShoppingCart
{
    public class ShoppingCartDto
    {
        [Required(ErrorMessage = "Shopping cart ID is required.")]
        public int ShoppingCartId { get; set; }

        [Required(ErrorMessage = "Customer ID is required.")]
        public string CustomerId { get; set; } // Customer's ID

        [Required(ErrorMessage = "Created date is required.")]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "Items are required.")]
        public List<ShoppingCartItemDto> Items { get; set; } // List of cart items
    }
}
