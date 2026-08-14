namespace Domain.Orders.Exceptions;

public sealed class OrderItemNotFoundException:Exception
{
    public OrderItemNotFoundException( string message):base(message)    
    {
        
    }
}