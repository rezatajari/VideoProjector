using VideoProjector.Common;
using VideoProjector.DTOs;

namespace VideoProjector.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseCenter<string>> Login(LoginDto loginDto);
    }
}
