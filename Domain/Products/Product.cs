using Domain.Abstractions;

namespace Domain.Products;

public sealed class Product:BaseEntity
{
    public Name Name { get; private init; } =new Name(string.Empty);
    public Description? Description { get; private init; }
}