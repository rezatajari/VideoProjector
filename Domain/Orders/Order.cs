using Domain.Abstractions;
using Domain.Orders.Enums;
using Domain.Orders.Exceptions;
using Domain.Shared;

namespace Domain.Orders;

public sealed class Order : BaseEntity
{
    private readonly List<OrderItem> _items = [];

    public Guid UserId { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public Money? TotalPrice { get; private set; }
    public OrderStatus Status { get; private set; }

    private Order(Guid userId)
    {
        UserId = userId;
        Status = OrderStatus.Pending;
    }

    private Order()
    {
    }

    public static Order Create(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty.", nameof(userId));

        return new Order(userId);
    }

    public OrderItem AddSaleItem(
        Guid productId,
        int quantity,
        Money unitPrice)
    {
        EnsureItemsCanBeModified();

        var item = OrderItem.CreateForSale(Id, productId, quantity, unitPrice);
        AddItem(item);

        return item;
    }

    public OrderItem AddRentalItem(
        Guid productId,
        DateRange duration,
        int quantity,
        Money unitPricePerDay,
        DateOnly today)
    {
        EnsureItemsCanBeModified();
        ArgumentNullException.ThrowIfNull(duration);

        if (duration.Start < today)
            throw new InvalidDateRangeException("Rental start date cannot be in the past.");

        var item = OrderItem.CreateForRental(
            Id,
            productId,
            duration,
            quantity,
            unitPricePerDay);
        AddItem(item);

        return item;
    }

    public void ChangeItemQuantity(Guid orderItemId, int quantity)
    {
        EnsureItemsCanBeModified();

        var item = GetItem(orderItemId);
        item.ChangeQuantity(quantity);
        RecalculateTotalPrice();
    }

    public void RemoveItem(Guid orderItemId)
    {
        EnsureItemsCanBeModified();

        var item = GetItem(orderItemId);
        _items.Remove(item);
        RecalculateTotalPrice();
    }

    public void Confirm()
    {
        EnsureStatus(OrderStatus.Pending, OrderStatus.Confirmed);

        if (_items.Count == 0)
            throw new EmptyOrderException("An order must contain at least one item before confirmation.");

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

    private void AddItem(OrderItem item)
    {
        if (TotalPrice is not null && TotalPrice.Currency != item.LineTotal.Currency)
            throw new InvalidOrderCurrencyException(
                "All items in an order must use the same currency.");

        _items.Add(item);
        RecalculateTotalPrice();
    }

    private OrderItem GetItem(Guid orderItemId)
    {
        if (orderItemId == Guid.Empty)
            throw new ArgumentException("Order item ID cannot be empty.", nameof(orderItemId));

        return _items.Find(item => item.Id == orderItemId)
               ?? throw new OrderItemNotFoundException(
                   $"Order item '{orderItemId}' was not found in order '{Id}'.");
    }

    private void RecalculateTotalPrice()
    {
        if (_items.Count == 0)
        {
            TotalPrice = null;
            return;
        }

        var total = _items[0].LineTotal;
        for (var index = 1; index < _items.Count; index++)
            total += _items[index].LineTotal;

        TotalPrice = total;
    }

    private void EnsureItemsCanBeModified()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOrderStatusTransitionException(
                $"Order items cannot be modified while the order status is {Status}.");
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