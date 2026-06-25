using Microsoft.JSInterop;
using Shared.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace Client.Services;

public class ProductService(HttpClient http, IJSRuntime jsRuntime)
{
    public async Task<List<Product>> GetProductsAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/products");

        var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await http.SendAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return new List<Product>();
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Product>>() ?? new List<Product>();
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