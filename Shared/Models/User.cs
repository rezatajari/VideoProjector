namespace Shared.Models;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public required string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = "Customer";
}
