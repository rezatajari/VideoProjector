using Domain.Abstractions;
using Domain.Categories;

namespace Domain.Products;

public sealed class Product:BaseEntity
{
    public Guid CategoryId { get; init; }
    public Name Name { get; private init; } =new Name(string.Empty);
    public Description? Description { get; private init; }
    
    
    public ICollection<Category>  Categories { get; private init; } = new HashSet<Category>();
}