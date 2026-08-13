namespace Domain.Categories.Exceptions;

public sealed class InvalidCategoryNameException : Exception
{
    public InvalidCategoryNameException(string message)
        : base(message)
    {
    }
}