using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "وارد کردن ایمیل الزامی است.")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "وارد کردن رمز عبور الزامی است.")]
    public string Password { get; set; } = string.Empty;
}