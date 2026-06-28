using Microsoft.JSInterop;
using Shared.Models;
using System.Net.Http.Json;

namespace Client.Services;

public class OrderService(HttpClient http)
{

    public async Task<List<Order>> GetOrdersAsync()
    {
        var hasToken = http.DefaultRequestHeaders.Authorization != null;
        var orders = await http.GetFromJsonAsync<List<Order>>("api/orders");
        return orders ?? [];
    }
    public async Task<bool> UpdateOrderStatusAsync(int id, OrderStatus newStatus)
    {
        var response = await http.PutAsJsonAsync($"api/orders/{id}/status", newStatus);
        return response.IsSuccessStatusCode;
    }
}