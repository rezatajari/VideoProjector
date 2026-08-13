namespace Domain.Users.Exceptions;

public sealed class InvalidFullNameException : Exception
{
    public InvalidFullNameException(string message) : base(message)
    {
    }
}