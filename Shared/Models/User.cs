using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }

    public string Role { get; set; } = "Customer";
}
