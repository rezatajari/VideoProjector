using Domain.Abstractions;

namespace Domain.Categories;

public sealed class Category:BaseEntity
{
    public CategoryName CategoryName { get;private set; }

    private Category(CategoryName categoryName)
    {
        CategoryName = categoryName;    
    }

    public static Category Create(CategoryName categoryName)
    {
        return new Category(categoryName); 
    }
}