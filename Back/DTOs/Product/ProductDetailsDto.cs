namespace Back.DTOs.Product;
public record ProductDetailsDto(
    int ProductId,
    string? Name,
    string? Description,
    decimal Price,
    int StockQuantity,
    string? ImageUrl);
