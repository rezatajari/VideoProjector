using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "نام و نام خانوادگی الزامی است.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "ایمیل الزامی است.")]
    [EmailAddress(ErrorMessage = "فرمت ایمیل وارد شده صحیح نیست.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "رمز عبور الزامی است.")]
    [MinLength(6, ErrorMessage = "رمز عبور باید حداقل ۶ کاراکتر باشد.")]
    public string Password { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
}