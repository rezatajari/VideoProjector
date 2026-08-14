using Domain.Categories.Exceptions;

namespace Domain.Categories.ValueObjects;

public sealed record CategoryName
{
    private const int MaxLength = 50;

    public string Value { get; }

    public CategoryName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidCategoryNameException(
                "Category name cannot be empty.");
        }

        var normalizedValue = value.Trim();

        if (normalizedValue.Length > MaxLength)
        {
            throw new InvalidCategoryNameException(
                $"Category name cannot exceed {MaxLength} characters.");
        }

        Value = normalizedValue;
    }
}
