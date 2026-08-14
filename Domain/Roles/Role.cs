using Domain.Abstractions;

namespace Domain.Roles;

public class Role:BaseEntity
{
    public string Name { get; private set; }

    private Role(string name)
    {
        Name = name;
    }

    public static Role Create(string name)
    {
        Role role = new Role(name);
        return role;
    }
}