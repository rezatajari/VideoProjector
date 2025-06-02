using System.ComponentModel.DataAnnotations;

namespace VideoProjector.DTOs.Order
{
    public class AdminOrderUpdateDto
    {
        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Order status is required.")]
        [MaxLength(50, ErrorMessage = "Order status cannot be longer than 50 characters.")]
        public string OrderStatus { get; set; }  // E.g., "Pending", "Shipped", "Cancelled"

        public DateTime? ShippingDate { get; set; }
    }
}
