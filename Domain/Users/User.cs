using Domain.Abstractions;
using Domain.Roles;
using Domain.Users.Exceptions;
using Domain.Users.ValueObjects;

namespace Domain.Users;

public sealed class User:BaseEntity
{
    private readonly List<Role> _roles = [];
    public FullName FullName { get; private set; }
    public Email Email { get;private set; }
    public PasswordHash PasswordHash { get; private set; }
    public PhoneNumber? PhoneNumber { get; private set; }
    public ICollection<Role> Roles => _roles.AsReadOnly();


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

    public void AddRole(Role role)
    {
        if (role == null)
            throw new InvalidRoleException("Role is null.");
        
        if (_roles.Contains(role))
            throw new InvalidRoleException("Role already exists.");
        
        _roles.Add(role);
    }
}