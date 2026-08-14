using Domain.Roles.Exceptions;

namespace Domain.Roles;

public sealed record RoleName
{
    private const int MaxLength = 50;

    public string Value { get; }

    public RoleName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidRoleNameException("Role name cannot be empty.");

        var normalizedValue = value.Trim();
        if (normalizedValue.Length > MaxLength)
            throw new InvalidRoleNameException("Role name is too long.");
        
        Value = normalizedValue;
    }
}