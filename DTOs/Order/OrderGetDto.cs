using System.ComponentModel.DataAnnotations;
using VideoProjector.DTOs.OrderDetail;

namespace VideoProjector.DTOs.Order
{
    public class OrderGetDto
    {
        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Customer ID is required.")]
        public string CustomerId { get; set; }

        [Required(ErrorMessage = "Order date is required.")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Total amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Order status is required.")]
        [MaxLength(50, ErrorMessage = "Order status cannot be longer than 50 characters.")]
        public string OrderStatus { get; set; }

        [Required(ErrorMessage = "Shipping address is required.")]
        [MaxLength(250, ErrorMessage = "Shipping address cannot be longer than 250 characters.")]
        public string ShippingAddress { get; set; }

        public DateTime? ShippingDate { get; set; }

        [Required(ErrorMessage = "Order details are required.")]
        public List<OrderDetailDto> OrderDetails { get; set; } // List of items
    }
}
