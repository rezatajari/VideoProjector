﻿using VideoProjector.Common;
using VideoProjector.DTOs.Account;

namespace VideoProjector.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseCenter<string>> Login(LoginDto loginDto);
        Task<ResponseCenter<string>> Register(RegisterDto registerDto);
        Task<ResponseCenter<string>> ConfirmEmail(string customerId, string token);
    }
}
