using Domain.Abstractions;
using Domain.Categories.Exceptions;
using Domain.Products.Exceptions;
using Domain.Products.ValueObjects;
using Domain.Shared;

namespace Domain.Products;

public sealed class Product:BaseEntity
{
    public Guid? CategoryId  { get;private set; }
    public ProductName ProductName { get; private set; } 
    public ProductDescription? ProductDescription { get; private set; }
    public Money? SalePrice { get; private set; }
    public int QuantityForSale { get; private set; }
    public Money? RentalPricePerDay { get; private set; }
    public int QuantityForRental { get; private set; }
    public string ImageUrl { get;private set; } = "/images/default-projector.png"; 
    public string? TestVideoUrl { get;private set; }

    
    private Product(
        ProductName productName,
        ProductDescription? productDescription,
        string imageUrl,
        string? testVideoUrl
    )
    {
        ProductName = productName;  
        ProductDescription = productDescription;
        ImageUrl = imageUrl;
        TestVideoUrl = testVideoUrl;
    }

    public static Product Create(
        ProductName productName,
        ProductDescription? productDescription,
        string imageUrl,
        string? testVideoUrl
        )
    {
        return new Product(
            productName,
            productDescription,
            imageUrl,
            testVideoUrl);
    }

    
    public void AssignCategory(Guid categoryId)
    {
        if (categoryId==Guid.Empty)
            throw new InvalidCategoryIdException("Category id  cannot be empty");
        CategoryId = categoryId;
    }

    public void RemoveCategory()
    {
        CategoryId = null;
    }

    public void EnableForSale(Money salePrice, int quantity)
    {
        if (quantity<=0)
            throw new InvalidQuantityException("Quantity cannot be negative.");
        
        ArgumentNullException.ThrowIfNull(salePrice);

        SalePrice = salePrice;
        QuantityForSale = quantity;
    }

    public void DisableForSale()
    {
        SalePrice = null;
        QuantityForSale = 0;
    }

    public void EnableForRental(Money rentalPricePerDay, int quantity)
    {
        if (quantity<=0)
            throw new InvalidQuantityException("Quantity cannot be negative.");
        
        ArgumentNullException.ThrowIfNull(rentalPricePerDay);

        RentalPricePerDay = rentalPricePerDay;
        QuantityForRental = quantity;
    }

    public void DisableForRental()
    {
        RentalPricePerDay = null;
        QuantityForRental = 0;
    }
}