namespace VideoProjector.DTOs.Product
{
    public class ProductSearchDto
    {
        public required string SearchTerm { get; set; } // Keyword for search
        public int? CategoryId { get; set; }   // Filter by category
        public decimal? MinPrice { get; set; } // Minimum price filter
        public decimal? MaxPrice { get; set; } // Maximum price filter
    }

}
