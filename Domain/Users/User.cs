using Domain.Abstractions;
using Domain.Roles;
using Domain.Users.Exceptions;
using Domain.Users.ValueObjects;

namespace Domain.Users;

public sealed class User:BaseEntity
{
    private readonly List<Guid> _rolesId = [];
    public FullName FullName { get; private set; }
    public Email Email { get;private set; }
    public PasswordHash PasswordHash { get; private set; }
    public PhoneNumber? PhoneNumber { get; private set; }
    public ICollection<Guid> RolesId => _rolesId.AsReadOnly();


    private User(
        FullName fullName, 
        Email email,
        PasswordHash passwordHash, 
        PhoneNumber? phoneNumber)
    {
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        PhoneNumber = phoneNumber;
    }

    public static User Create(
        FullName fullName,
        Email email,
        PasswordHash passwordHash,
        PhoneNumber? phoneNumber)
    {
        User user = new User(fullName, email, passwordHash, phoneNumber);
        return user;
    }

    public void AddRole(Guid roleId)
    {
        if (roleId == Guid.Empty)
            throw new InvalidRoleException("Role is null.");
                
        bool isRoleExist=_rolesId.Any(r => r == roleId);
        
        if (isRoleExist)
            throw new InvalidRoleException("Role already exists.");
        
        _rolesId.Add(roleId);
    }

    public void RemoveRole(Guid roleId)
    {
        if (roleId == Guid.Empty)
            throw new InvalidRoleException("Role is null.");
        
        bool isRoleExist=_rolesId.Any(r => r == roleId);
        if (!isRoleExist)
            throw new InvalidRoleException("Role already doesn't exists.");
        
        _rolesId.Remove(roleId);
    }
}