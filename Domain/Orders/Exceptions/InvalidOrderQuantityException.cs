namespace Domain.Orders.Exceptions;

public class InvalidOrderQuantityException:Exception
{
    public InvalidOrderQuantityException(string message) : base(message)
    {
        
    }
}