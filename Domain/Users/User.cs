using Domain.Abstractions;
using Domain.Roles;
using Domain.Users.ValueObjects;

namespace Domain.Users;

public sealed class User:BaseEntity
{
    public FullName FullName { get; private set; }
    public Email Eamil { get;private set; }
    public PasswordHash PasswordHash { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public List<Role> Roles { get; private set; }


    private User(
        FullName fullName, 
        Email email,
        PasswordHash passwordHash, 
        PhoneNumber phoneNumber,
        Guid roleId)
    {
        FullName = fullName;
        Eamil = email;
        PasswordHash = passwordHash;
        PhoneNumber = phoneNumber;
        RoleId = roleId;
    }

    public static User Create(
        FullName fullName,
        Email email,
        PasswordHash passwordHash,
        PhoneNumber phoneNumber,
        Guid roleId)
    {
        User user = new User(fullName, email, passwordHash, phoneNumber,roleId);
    }
    
}