using Domain.Abstractions;
using Domain.Roles;
using Domain.Users.ValueObjects;

namespace Domain.Users;

public sealed class User:BaseEntity
{
    public FullName FullName { get; private set; }
    public Email Email { get;private set; }
    public PasswordHash PasswordHash { get; private set; }
    public PhoneNumber? PhoneNumber { get; private set; }
    public List<Role> Roles { get; private set; }


    private User(
        FullName fullName, 
        Email email,
        PasswordHash passwordHash, 
        PhoneNumber? phoneNumber,
        List<Role> roles)
    {
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        PhoneNumber = phoneNumber;
        Roles = roles;
    }

    public static User Create(
        FullName fullName,
        Email email,
        PasswordHash passwordHash,
        PhoneNumber? phoneNumber,
        List<Role> roles)
    {
        User user = new User(fullName, email, passwordHash, phoneNumber,roles);
        return user;
    }
    
}