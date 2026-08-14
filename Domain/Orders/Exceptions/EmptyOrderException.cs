namespace Domain.Orders.Exceptions;

public sealed class EmptyOrderException:Exception
{
    public EmptyOrderException(string message) : base(message)
    {
        
    }
}