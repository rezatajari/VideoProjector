namespace Domain.Shared.Exceptions;

public sealed class InvalidMoneyAmountException:Exception
{
    public InvalidMoneyAmountException(string message):base(message){}
}