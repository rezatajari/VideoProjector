using Shared.Models;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace Client.Services;

public class ProductService(HttpClient http)
{
    public async Task<List<Product>> GetProductsAsync()
    {
        var products = await http.GetFromJsonAsync<List<Product>>("api/products");
        return products ?? [];
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await http.GetFromJsonAsync<Product>($"api/products/{id}");
    }

    public async Task<HttpResponseMessage> CreateOrderAsync(Order order)
    {
        return await http.PostAsJsonAsync("api/orders", order);
    }
}