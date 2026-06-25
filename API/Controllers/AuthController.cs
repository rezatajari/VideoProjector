using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Models;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(VideoProjectorDbContext context, TokenService tokenService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<string>> Register(RegisterDto dto)
    {
        if (await context.Users.AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower()))
        {
            return BadRequest("این ایمیل قبلاً در سیستم ثبت شده است.");
        }

        using var hmac = new HMACSHA512();

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email.ToLower(),
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = Convert.ToBase64String(hmac.Key) + ":" +
                           Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)))
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();
        string token = tokenService.CreateToken(user);

        return Ok(token);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginDto dto)
    {
        // ۱. پیدا کردن کاربر بر اساس ایمیل
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower());
        if (user == null)
        {
            return Unauthorized("ایمیل یا رمز عبور اشتباه است.");
        }

        // ۲. بازسازی هش و مقایسه با پسورد ذخیره شده
        var parts = user.PasswordHash.Split(':');
        if (parts.Length != 2) return Unauthorized("ساختار رمز عبور در دیتابیس معتبر نیست.");

        var key = Convert.FromBase64String(parts[0]);
        var hash = Convert.FromBase64String(parts[1]);

        using var hmac = new HMACSHA512(key);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dto.Password));

        // مقایسه آرایه‌های بایت
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != hash[i]) return Unauthorized("ایمیل یا رمز عبور اشتباه است.");
        }

        // ۳. صدور توکن در صورت صحت مشخصات
        return Ok(tokenService.CreateToken(user));
    }
}