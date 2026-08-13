using Domain.Products.Exceptions;

namespace Domain.Products.ValueObjects;

public sealed record ProductName
{
    private const int MaxLength = 100;
    public string Value { get; }

    public ProductName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidProductNameException(
                "Product name cannot be null or whitespace.");
        }

        string normalizedValue = value.Trim();

        if (normalizedValue.Length > MaxLength)
        {
            throw new InvalidProductNameException(
                "Product name cannot be longer than " + MaxLength + " characters.");
        }
        
        Value = normalizedValue;
    }
    
}
