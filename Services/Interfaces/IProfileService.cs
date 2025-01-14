using Microsoft.AspNetCore.Identity;
using VideoProjector.Common;
using VideoProjector.DTOs;

namespace VideoProjector.Services.Interfaces
{
    public interface IProfileService
    {
        Task<ResponseCenter<ProfileDto>> GetCustomerProfileById(string customerId);

        Task<ResponseCenter<EditDto>> GetEditProfile(string customerId);
        Task<ResponseCenter<IdentityResult>> EditProfile(EditDto editDto, string customerId);

        Task<ResponseCenter<IdentityResult>> UpdatePassword(UpdatePasswordDto updatePassword, string customerId);


    }
}
