namespace Domain.Products.Exceptions;

public sealed class InvalidProductNameException:Exception
{
    public InvalidProductNameException(string message)
        :base(message){}
}