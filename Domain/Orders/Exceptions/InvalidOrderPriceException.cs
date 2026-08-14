namespace Domain.Orders.Exceptions;

public class InvalidOrderPriceException:Exception
{
    public InvalidOrderPriceException(string message) : base(message)
    {
        
    }
}