namespace Domain.Users.ValueObjects;

using Domain.Users.Exceptions;

public sealed record PasswordHash
{
    public string Value { get; }

    public PasswordHash(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidPasswordHashException("Password hash cannot be empty.");

        Value = value;
    }
}