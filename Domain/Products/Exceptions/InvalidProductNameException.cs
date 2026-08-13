namespace Domain.Products.Exceptions;

public class InvalidProductNameException:Exception
{
    public InvalidProductNameException(string message)
        :base(message){}
}