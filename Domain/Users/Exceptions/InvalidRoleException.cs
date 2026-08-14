using System.Diagnostics.Contracts;

namespace Domain.Users.Exceptions;

public class InvalidRoleException:Exception
{
    public InvalidRoleException(string message):base(message)
    {
        
    }
}