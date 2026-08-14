using Domain.Abstractions;
using Domain.Orders.Enums;
using Domain.Orders.Exceptions;
using Domain.Products.Exceptions;
using Domain.Shared;

namespace Domain.Orders;

public sealed class Order : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid ProductId { get; private set; }
    public DateRange? Duration { get; private set; }
    public int Quantity { get; private set; }
    public Money PricePerUnit { get; private set; }
    public Money TotalPrice { get; private set; }
    public OrderStatus Status { get; private set; }


    private Order(Guid userId, Guid productId, int quantity, Money pricePerUnit, Money totalPrice,
        DateRange? duration = default)
    {
        Status = OrderStatus.Pending;
        UserId = userId;
        ProductId = productId;
        Duration = duration;
        Quantity = quantity;
        PricePerUnit = pricePerUnit;
        TotalPrice = totalPrice;
    }

    public static Order CreateForRental(
        Guid userId,
        Guid productId,
        DateRange duration,
        int quantity,
        Money pricePerUnitForPerDay)
    {
        
        ArgumentNullException.ThrowIfNull(duration);
        if (duration.NumberOfDays <= 1)
            throw new InvalidDateRangeException("Rental duration must be greater than or equal to 1.");

        if (userId == Guid.Empty || productId == Guid.Empty)
            throw new ArgumentException("User ID or Product ID cannot be empty.");
        if (quantity <= 0)
            throw new InvalidQuantityException("Quantity must be greater than zero");
        ArgumentNullException.ThrowIfNull(pricePerUnitForPerDay);

        Money totalPrice = pricePerUnitForPerDay * quantity * duration.NumberOfDays;
        Order order = new Order(userId, productId, quantity, pricePerUnitForPerDay, totalPrice, duration);

        return order;
    }

    public static Order CreateForSale(Guid userId, Guid productId, int quantity, Money pricePerUnit)
    {
        if (userId == Guid.Empty || productId == Guid.Empty)
            throw new ArgumentException("User ID or Product ID cannot be empty.");
        if (quantity <= 0)
            throw new InvalidQuantityException("Quantity must be greater than zero");
        if (pricePerUnit == null)
            throw new ArgumentException("Price per Unit cannot be null.");

        Money totalPrice = pricePerUnit * quantity;
        Order order = new Order(userId, productId, quantity, pricePerUnit, totalPrice);
        return order;
    }


    public void Confirm()
    {
        Status = OrderStatus.Confirmed;
    }

    public void Cancel()
    {
        Status = OrderStatus.Canceled;
    }

    public void MarkDelivered()
    {
        Status = OrderStatus.Delivered;
    }

    public void Complete()
    {
        Status = OrderStatus.Completed;
    }
}