using VideoProjector.Common;
using VideoProjector.DTOs.Account;

namespace VideoProjector.Services.Interfaces
{
    public interface IAccountService
    {
        Task<GeneralResponse<string>> Login(LoginDto loginDto);
        Task<GeneralResponse<string>> Register(RegisterDto registerDto);
        Task<GeneralResponse<string>> ConfirmEmail(string customerId, string token);
    }
}
