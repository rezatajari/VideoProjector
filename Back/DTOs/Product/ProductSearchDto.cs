using System.ComponentModel.DataAnnotations;

namespace VideoProjector.DTOs.Product
{
    public class ProductSearchDto
    {
        [Required(ErrorMessage = "Search term is required.")]
        [MaxLength(100, ErrorMessage = "Search term cannot be longer than 100 characters.")]
        public string SearchTerm { get; set; } // Keyword for search

        public int? CategoryId { get; set; }   // Filter by category

        [Range(0.01, double.MaxValue, ErrorMessage = "Minimum price must be greater than zero.")]
        public decimal? MinPrice { get; set; } // Minimum price filter

        [Range(0.01, double.MaxValue, ErrorMessage = "Maximum price must be greater than zero.")]
        public decimal? MaxPrice { get; set; } // Maximum price filter
    }
}
