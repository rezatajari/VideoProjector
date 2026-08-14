using Domain.Abstractions;
using Domain.Users.ValueObjects;

namespace Domain.Users;

public sealed class User:BaseEntity
{
    public Guid RoleId { get; private set; }
    public FullName FullName { get; private set; }
    public Email Eamil { get;private set; }
    public PasswordHash PasswordHash { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    
}