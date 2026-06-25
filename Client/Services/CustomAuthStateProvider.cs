using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Client.Services;

public class CustomAuthStateProvider(HttpClient http, IJSRuntime jsRuntime) : AuthenticationStateProvider
{

    private readonly AuthenticationState _anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

        if (string.IsNullOrEmpty(token))
        {
            return _anonymous;
        }

        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
    }

    public void NotifyUserAuthentication(string token)
    {
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(_anonymous);
        NotifyAuthenticationStateChanged(authState);
        http.DefaultRequestHeaders.Authorization = null;
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        if (keyValuePairs != null)
        {
            if (keyValuePairs.TryGetValue("role", out var roles) ||
                keyValuePairs.TryGetValue(ClaimTypes.Role, out roles))
            {
                if (roles != null)
                {
                    var rolesString = roles.ToString()!.Trim();

                    if (rolesString.StartsWith('['))
                    {
                        var parsedRoles = JsonSerializer.Deserialize<string[]>(rolesString);
                        foreach (var parsedRole in parsedRoles!)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                        }
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, rolesString));
                    }
                }

                keyValuePairs.Remove("role");
                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => {
                if (kvp.Key == "unique_name" || kvp.Key == "name")
                    return new Claim(ClaimTypes.Name, kvp.Value.ToString()!);
                if (kvp.Key == "sub" || kvp.Key == "nameid")
                    return new Claim(ClaimTypes.NameIdentifier, kvp.Value.ToString()!);

                return new Claim(kvp.Key, kvp.Value.ToString()!);
            }));
        }

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}