namespace Domain.Roles;

public sealed record RoleName
{
    public RoleName(string value)
    {
        if  (string.IsNullOrWhiteSpace(value))
            throw new InvalidRoleNameException("Role name cannot be empty.");
    }
}