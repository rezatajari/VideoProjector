using System.ComponentModel.DataAnnotations;

namespace VideoProjector.DTOs.Product
{
    public class ProductListDto
    {
        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }

        [MaxLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Url(ErrorMessage = "Invalid URL format.")]
        public string? ImageUrl { get; set; }

        [MaxLength(100, ErrorMessage = "Category name cannot be longer than 100 characters.")]
        public string? CategoryName { get; set; } // Include category name for convenience
    }
}
