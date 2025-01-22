using System.ComponentModel.DataAnnotations;

namespace VideoProjector.DTOs.OrderDetail
{
    public class OrderDetailAddDto
    {
        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }  // The ID of the product being ordered

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }   // The quantity of the product being ordered

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }  // The price of the product at the time of the order
    }
}
