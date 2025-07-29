using Front.DTOs.Product;

namespace Front.DTOs.Category;

public class CategoryDetailsDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int TotalProducts { get; set; }
    public string? ImageUrl { get; set; }
    public List<ProductSummaryDto> Products { get; set; } = [];
}