namespace Domain.Users.Exceptions;

public sealed class InvalidPasswordHashException : Exception
{
    public InvalidPasswordHashException(string message) : base(message) { }
}