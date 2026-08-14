using Domain.Users.Exceptions;

namespace Domain.Users.ValueObjects;

public record class FullName
{
    private const int MaxLength = 100;
    
    public string Value { get; }

    public FullName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidFullNameException("Full name cannot be null or empty.");

        var normalizedValue = value.Trim();
        if (normalizedValue.Length > MaxLength)
            throw new InvalidFullNameException("Full name cannot exceed 100 characters.");
        
        Value = normalizedValue;
    }
}