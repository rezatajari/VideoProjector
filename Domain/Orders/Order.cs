using Domain.Abstractions;
using Domain.Orders.Enums;
using Domain.Orders.Exceptions;
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
    public bool IsRental => Duration is not null;


    private Order(
        Guid userId,
        Guid productId,
        int quantity,
        Money pricePerUnit,
        DateRange? duration = null)
    {
        Status = OrderStatus.Pending;
        UserId = userId;
        ProductId = productId;
        Duration = duration;
        Quantity = quantity;
        PricePerUnit = pricePerUnit;
        TotalPrice = duration is null
            ? pricePerUnit * quantity
            : pricePerUnit * quantity * duration.NumberOfDays;
    }

    public static Order CreateForRental(
        Guid userId,
        Guid productId,
        DateRange duration,
        int quantity,
        Money pricePerUnitPerDay)
    {
        ArgumentNullException.ThrowIfNull(duration);
        ValidateCreationArguments(userId, productId, quantity, pricePerUnitPerDay);

        return new Order(userId, productId, quantity, pricePerUnitPerDay, duration);
    }

    public static Order CreateForSale(Guid userId, Guid productId, int quantity, Money pricePerUnit)
    {
        ValidateCreationArguments(userId, productId, quantity, pricePerUnit);

        return new Order(userId, productId, quantity, pricePerUnit);
    }

    public void Confirm()
    {
        EnsureStatus(OrderStatus.Pending, OrderStatus.Confirmed);
        Status = OrderStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status is not OrderStatus.Pending and not OrderStatus.Confirmed)
            throw InvalidTransitionTo(OrderStatus.Canceled);

        Status = OrderStatus.Canceled;
    }

    public void MarkDelivered()
    {
        EnsureStatus(OrderStatus.Confirmed, OrderStatus.Delivered);
        Status = OrderStatus.Delivered;
    }

    public void Complete()
    {
        EnsureStatus(OrderStatus.Delivered, OrderStatus.Completed);
        Status = OrderStatus.Completed;
    }

    private static void ValidateCreationArguments(
        Guid userId,
        Guid productId,
        int quantity,
        Money pricePerUnit)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        if (productId == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));

        if (quantity <= 0)
            throw new InvalidOrderQuantityException("Order quantity must be greater than zero.");

        ArgumentNullException.ThrowIfNull(pricePerUnit);

        if (pricePerUnit.Amount <= 0)
            throw new InvalidOrderPriceException("Order price must be greater than zero.");
    }

    private void EnsureStatus(OrderStatus requiredStatus, OrderStatus targetStatus)
    {
        if (Status != requiredStatus)
            throw InvalidTransitionTo(targetStatus);
    }

    private InvalidOrderStatusTransitionException InvalidTransitionTo(OrderStatus targetStatus)
    {
        return new InvalidOrderStatusTransitionException(
            $"Order cannot transition from {Status} to {targetStatus}.");
    }
}