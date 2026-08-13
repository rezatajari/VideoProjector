namespace Domain.Products.Exceptions;

public sealed class InvalidProductDescriptionException:Exception
{
    public InvalidProductDescriptionException(string message) : base(message) { } 
}