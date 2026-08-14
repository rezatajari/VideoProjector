using Domain.Abstractions;
using Domain.Roles.ValueObjects;

namespace Domain.Roles;

public class Role:BaseEntity
{
    public RoleName RoleName { get; private set; }

    private Role(RoleName roleName)
    {
        RoleName = roleName;
    }

    public static Role Create(RoleName RoleName)
    {
        Role role = new Role(RoleName);
        return role;
    }
}