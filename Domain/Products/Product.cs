using Domain.Abstractions;
using Domain.Categories;
using Domain.Categories.Exceptions;
using Domain.Products.ValueObjects;

namespace Domain.Products;

public sealed class Product:BaseEntity
{
    public Guid? CategoryId  { get;private set; }
    public ProductName ProductName { get; private set; } 
    public ProductDescription? ProductDescription { get; private set; }
    
    
    private Product(ProductName productName, ProductDescription? productDescription)
    {
        ProductName = productName;  
        ProductDescription = productDescription;
    }

    public static Product Create(ProductName productName, ProductDescription? productDescription)
    {
        return new Product(productName, productDescription);
    }

    public void AssignCategory(Guid categoryId)
    {
        if (categoryId==Guid.Empty)
            throw new InvalidCategoryIdException("Category id  cannot be empty");
        CategoryId = categoryId;
    }

    public void RemoveCategory(Guid categoryId)
    {
        CategoryId = null;
    }
}