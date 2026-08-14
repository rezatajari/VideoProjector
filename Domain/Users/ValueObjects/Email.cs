using System.Reflection.Emit;
using Domain.Users.Exceptions;

namespace Domain.Users.ValueObjects;

public class Email
{
    private const int MaxLength = 100;

    public string Value { get; }
    
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value))
            throw new InvalidEmailException("Email cannot be null or empty.");
        
        var normalizedValue = value.Trim();
        if (normalizedValue.Length > MaxLength)
            throw new InvalidEmailException("Full name cannot exceed 100 characters.");
        
        
    }
}