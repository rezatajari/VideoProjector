namespace Domain.Products.Exceptions;

public class InvalidProductDescriptionException:Exception
{
    public InvalidProductDescriptionException(string message) : base(message) { } 
}