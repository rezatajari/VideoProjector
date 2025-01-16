namespace VideoProjector.DTOs.Product
{
    public class ProductListDto
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? CategoryName { get; set; } // Include category name for convenience
    }

}
