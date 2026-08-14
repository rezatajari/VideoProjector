namespace Domain.Users.Exceptions;

public sealed class InvalidPhoneNumberException:Exception
{
    public InvalidPhoneNumberException(string message):base(message)
    {
        
    }
}