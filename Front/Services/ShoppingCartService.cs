using Front.DTOs.ShoppingCart;

namespace Front.Services;

public class ShoppingCartService
{
    private readonly List<ShoppingCartItemDto> _items = [];

    public IReadOnlyList<ShoppingCartItemDto> Items => _items;

    // Event that will be triggered when the cart changes
    public event Action? OnChange;
    public void AddItem(ShoppingCartItemDto newItem)
    {
        var existingItem = _items.FirstOrDefault(x => x.ProductId == newItem.ProductId);
        if (existingItem != null)
            existingItem.Quantity += newItem.Quantity;
        else
            _items.Add(newItem);

        OnChange?.Invoke(); // Trigger the event
    }

    public void ClearItemList()
    {
        _items.Clear();
        OnChange?.Invoke();
    }

    public void RemoveItem(int productId)
    {
        var item = _items.FirstOrDefault(x => x.ProductId == productId);
        if (item != null)
        {
            _items.Remove(item);
            OnChange?.Invoke();
        }
    }

    public int TotalItems => _items.Sum(i => i.Quantity);
    public decimal TotalPrice => _items.Sum(i => i.TotalPrice);
}