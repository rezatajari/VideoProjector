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

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email.ToLower(),
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.Customer
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return Ok(tokenService.CreateToken(user));
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginDto dto)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower());
        if (user == null)
        {
            return Unauthorized("ایمیل یا رمز عبور اشتباه است.");
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return Unauthorized("ایمیل یا رمز عبور اشتباه است.");
        }

        return Ok(tokenService.CreateToken(user));
    }
}