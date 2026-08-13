namespace Domain.Shared.Exceptions;

public sealed class InvalidMatchCurrencyException:Exception
{
    public InvalidMatchCurrencyException(string message):base(message) 
    {
        
    }
}