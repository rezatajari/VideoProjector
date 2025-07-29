using System.ComponentModel.DataAnnotations;

namespace VideoProjector.DTOs.Order
{
    public class OrderListDto
    {
        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Order date is required.")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Total amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Order status is required.")]
        [MaxLength(50, ErrorMessage = "Order status cannot be longer than 50 characters.")]
        public string OrderStatus { get; set; }
    }
}
