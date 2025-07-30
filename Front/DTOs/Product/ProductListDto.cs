namespace Front.DTOs.Product;
public record ProductListDto(
    int ProductId,
    string? Name,
    decimal Price,
    string? ImageUrl
);
