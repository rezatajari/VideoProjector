using System.ComponentModel.DataAnnotations;

namespace VideoProjector.DTOs.Order
{
    public class CustomerOrderUpdateDto
    {
        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Shipping address is required.")]
        [MaxLength(250, ErrorMessage = "Shipping address cannot be longer than 250 characters.")]
        public string ShippingAddress { get; set; }
    }
}
