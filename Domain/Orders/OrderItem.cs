using Domain.Abstractions;
using Domain.Orders.Enums;
using Domain.Orders.Exceptions;
using Domain.Shared;

namespace Domain.Orders;

public sealed class OrderItem : BaseEntity
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public OrderItemType Type { get; private set; }
    public DateRange? Duration { get; private set; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; } = null!;
    public Money LineTotal { get; private set; } = null!;

    public bool IsRental => Type == OrderItemType.Rental;

    private OrderItem(
        Guid orderId,
        Guid productId,
        OrderItemType type,
        int quantity,
        Money unitPrice,
        DateRange? duration)
    {
        Validate(orderId, productId, quantity, unitPrice);

        if (type == OrderItemType.Rental && duration is null)
            throw new InvalidDateRangeException("A rental item requires a rental duration.");

        if (type == OrderItemType.Sale && duration is not null)
            throw new InvalidDateRangeException("A sale item cannot have a rental duration.");

        OrderId = orderId;
        ProductId = productId;
        Type = type;
        Duration = duration;
        Quantity = quantity;
        UnitPrice = unitPrice;
        LineTotal = CalculateLineTotal(unitPrice, quantity, duration);
    }

    private OrderItem()
    {
    }

    internal static OrderItem CreateForSale(
        Guid orderId,
        Guid productId,
        int quantity,
        Money unitPrice)
    {
        return new OrderItem(
            orderId,
            productId,
            OrderItemType.Sale,
            quantity,
            unitPrice,
            null);
    }

    internal static OrderItem CreateForRental(
        Guid orderId,
        Guid productId,
        DateRange duration,
        int quantity,
        Money unitPricePerDay)
    {
        ArgumentNullException.ThrowIfNull(duration);

        return new OrderItem(
            orderId,
            productId,
            OrderItemType.Rental,
            quantity,
            unitPricePerDay,
            duration);
    }

    internal void ChangeQuantity(int quantity)
    {
        ValidateQuantity(quantity);

        Quantity = quantity;
        LineTotal = CalculateLineTotal(UnitPrice, quantity, Duration);
    }

    private static void Validate(
        Guid orderId,
        Guid productId,
        int quantity,
        Money unitPrice)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException("Order ID cannot be empty.", nameof(orderId));

        if (productId == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));

        ValidateQuantity(quantity);
        ArgumentNullException.ThrowIfNull(unitPrice);

        if (unitPrice.Amount <= 0)
            throw new InvalidOrderPriceException("Order item price must be greater than zero.");
    }

    private static void ValidateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidOrderQuantityException(
                "Order item quantity must be greater than zero.");
    }

    private static Money CalculateLineTotal(
        Money unitPrice,
        int quantity,
        DateRange? duration)
    {
        var total = unitPrice * quantity;
        return duration is null ? total : total * duration.NumberOfDays;
    }
}