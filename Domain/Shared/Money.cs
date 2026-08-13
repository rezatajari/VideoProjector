using Domain.Shared.Exceptions;

namespace Domain.Shared;

public sealed record Money
{
    public decimal Amount { get; }
    
    public Currency Currency { get; }

    public Money(decimal amount, Currency currency)
    {
        if (amount < 0)
            throw new InvalidMoneyAmountException("Amount must be greater than zero");
        
        Amount = amount;
        Currency = currency;
    }

    public static Money operator+(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidMatchCurrencyException("Currency should be match");
        return new Money(left.Amount + right.Amount, right.Currency);
    }

    public static Money operator *(Money left, int quantity)
    {
        if (quantity <= 0)
            throw new InvalidMoneyAmountException("Quantity must be greater than zero");
        return new Money(left.Amount * quantity, left.Currency);
    }
}