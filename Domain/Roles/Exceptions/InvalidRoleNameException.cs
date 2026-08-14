namespace Domain.Roles.Exceptions;

public class InvalidRoleNameException:Exception
{
    public InvalidRoleNameException(string message):base(message)
    {
        
    }
}