using System.Net.Mail;
using Domain.Users.Exceptions;

namespace Domain.Users.ValueObjects;

public class Email
{
    private const int MaxLength = 100;

    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidEmailException("Email cannot be null or empty.");

        var normalizedValue = value.Trim().ToLowerInvariant();
        if (normalizedValue.Length > MaxLength)
            throw new InvalidEmailException("Email cannot exceed 100 characters.");

        bool isValid = MailAddress.TryCreate(normalizedValue, out MailAddress? mailAddress);
        
        if (mailAddress == null || !isValid)
            throw new InvalidEmailException("Invalid email address.");

        value = mailAddress.ToString();
    }
}