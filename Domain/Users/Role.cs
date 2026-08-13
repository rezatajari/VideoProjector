using Domain.Users.Exceptions;

namespace Domain.Users;

public sealed record Role
{
    public static readonly Role Admin = new("Admin");
    public static readonly Role Customer = new("Customer");

    public string Name { get; }

    private Role(string name) => Name = name;

    public static Role FromName(string name)
    {
        if (string.Equals(name, Admin.Name, StringComparison.OrdinalIgnoreCase))
            return Admin;

        if (string.Equals(name, Customer.Name, StringComparison.OrdinalIgnoreCase))
            return Customer;

        throw new InvalidRoleException($"Role '{name}' is not supported.");
    }
}