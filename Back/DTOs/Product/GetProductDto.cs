using System.ComponentModel.DataAnnotations;

namespace VideoProjector.DTOs.Product
{
    public class GetProductDto
    {
        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }
    }
}
