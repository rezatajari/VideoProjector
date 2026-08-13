namespace Domain.Categories.Exceptions;

public sealed class InvalidCategoryIdException:Exception
{
    public InvalidCategoryIdException(string message)
        :base(message)
    {
        
    }
}