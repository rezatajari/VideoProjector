using Domain.Abstractions;

namespace Domain.Categories;

public sealed class Category:BaseEntity
{
    public CategoryName CategoryName { get;private set; }
}