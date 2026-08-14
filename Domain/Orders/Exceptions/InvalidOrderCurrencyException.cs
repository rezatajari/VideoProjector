namespace Domain.Orders.Exceptions;

public sealed class InvalidOrderCurrencyException:Exception
{
        public InvalidOrderCurrencyException(string message):base(message)      
        {
                
        }
}