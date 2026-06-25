namespace Shared.Models;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty; // Projector, Screen, Accessory

    // بخش مربوط به فروش (اگر نال باشند یعنی کالا فروشی نیست)
    public decimal? SalePrice { get; set; }
    public int QuantityForSale { get; set; }

    // بخش مربوط به اجاره (اگر نال باشند یعنی کالا اجاره‌ای نیست)
    public decimal? RentalPricePerDay { get; set; }
    public int QuantityForRental { get; set; }
}