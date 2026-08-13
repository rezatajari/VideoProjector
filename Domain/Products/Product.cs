using Domain.Abstractions;
using Domain.Categories;

namespace Domain.Products;

public sealed class Product:BaseEntity
{
    public ProductName ProductName { get; private set; } = new ProductName(string.Empty);
    public Description? ProductDescription { get; private set; }
    
    
    
    public ICollection<Category>  Categories { get; private init; } = new HashSet<Category>();
}