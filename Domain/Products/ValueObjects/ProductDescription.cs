using Domain.Products.Exceptions;

namespace Domain.Products.ValueObjects;

public sealed record ProductDescription
{
    private const int MaxLength = 255;
    public string? Value { get; }

    public ProductDescription(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return;   
        
        string normalizedValue = value.Trim();
        if (normalizedValue.Length > MaxLength)
            throw new InvalidProductDescriptionException($"Value is too long. Maximum length is {MaxLength}");
        Value = normalizedValue;
    }
}