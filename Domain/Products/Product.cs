using Domain.Abstractions;
using Domain.Categories;
using Domain.Products.ValueObjects;

namespace Domain.Products;

public sealed class Product:BaseEntity
{
    public ProductName ProductName { get; private set; } 
    public ProductDescription? ProductDescription { get; private set; }
    
    
    public ICollection<Category>  Categories { get; private init; } = new HashSet<Category>();
}