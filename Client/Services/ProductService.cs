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

    public async Task<bool> DeleteProductAsync(int id)
    {
        var response = await http.DeleteAsync($"api/products/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<HttpResponseMessage> CreateProductAsync(Product product)
    {
        return await http.PostAsJsonAsync("api/products", product);
    }

    public async Task<HttpResponseMessage> UpdateProductAsync(int id, Product product)
    {
        return await http.PutAsJsonAsync($"api/products/{id}", product);
    }
}