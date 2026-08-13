using Domain.Abstractions;
using Domain.Categories.Exceptions;
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
    public Money RentalPricePerDay { get; private set; }
    public int QuantityForRental { get; private set; }
    
    
    private Product(
        ProductName productName,
        ProductDescription? productDescription,
        Money? salePrice,
        int quantityForSale,
        Money rentalPricePerDay,
        int quantityForRental
    )
    {
        ProductName = productName;  
        ProductDescription = productDescription;
        SalePrice = salePrice;
        QuantityForSale = quantityForSale;
        RentalPricePerDay = rentalPricePerDay;
        QuantityForRental = quantityForRental;
    }

    public static Product Create(
        ProductName productName,
        ProductDescription? productDescription,
        Money? salePrice,
        int quantityForSale,
        Money rentalPricePerDay,
        int quantityForRental
        )
    {
        return new Product(
            productName,
            productDescription,
            salePrice,
            quantityForSale,
            rentalPricePerDay,
            quantityForRental);
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
}