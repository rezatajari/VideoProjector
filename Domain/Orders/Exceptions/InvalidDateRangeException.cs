namespace Domain.Orders.Exceptions;

public sealed class InvalidDateRangeException:Exception
{
    public InvalidDateRangeException(string message) : base(message)    
    {
        
    }
}