namespace Back.DTOs.Product
{
    public class ProductSummaryDto
    {
        public string Name { get; set; }             
        public string? ShortDescription { get; set; }
        public decimal Price { get; set; }
        public int StockCount { get; set; }
    }
}
