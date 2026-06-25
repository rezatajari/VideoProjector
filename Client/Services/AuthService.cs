using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Shared.DTOs;
using System.Net.Http.Json;

namespace Client.Services;

public class AuthService(HttpClient http, IJSRuntime jsRuntime, AuthenticationStateProvider gameStateProvider)
{
    private readonly HttpClient _http = http;
    private readonly IJSRuntime _js = jsRuntime;
    private readonly CustomAuthStateProvider _authStateProvider = (CustomAuthStateProvider)gameStateProvider;

    public async Task<bool> RegisterAsync(RegisterDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", dto);
        if (!response.IsSuccessStatusCode) return false;

        var token = await response.Content.ReadAsStringAsync();
        await _js.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        _authStateProvider.NotifyUserAuthentication(token);

        return true;
    }

    public async Task<bool> LoginAsync(LoginDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", dto);
        if (!response.IsSuccessStatusCode) return false;

        var token = await response.Content.ReadAsStringAsync();
        await _js.InvokeVoidAsync("localStorage.setItem", "authToken", token);

        _authStateProvider.NotifyUserAuthentication(token);

        return true;
    }

    public async Task LogoutAsync()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
        _authStateProvider.NotifyUserLogout();
    }
}