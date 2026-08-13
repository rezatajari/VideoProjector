namespace Domain.Products.Exceptions;

public sealed class InvalidQuantityException:Exception
{
    public InvalidQuantityException(string message) : base(message)
    {
        
    }
}