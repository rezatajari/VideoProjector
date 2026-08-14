namespace Domain.Users.ValueObjects;

using Domain.Users.Exceptions;

public sealed record PhoneNumber
{
    private const int MaxLength = 20;
    public string Value { get; }

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidPhoneNumberException("Phone number cannot be empty.");

        var normalizedValue = value.Trim();
        if (normalizedValue.Length > MaxLength)
            throw new InvalidPhoneNumberException($"Phone number cannot exceed {MaxLength} characters.");

        Value = normalizedValue;
    }
}