using Microsoft.AspNetCore.Identity;
using VideoProjector.Common;
using VideoProjector.DTOs.Profile;

namespace VideoProjector.Services.Interfaces
{
    public interface IProfileService
    {
        Task<GeneralResponse<ProfileDto>> GetCustomerProfileById(string customerId);

        Task<GeneralResponse<EditDto>> GetEditProfile(string customerId);
        Task<GeneralResponse<IdentityResult>> EditProfile(EditDto editDto, string customerId);

        Task<GeneralResponse<IdentityResult>> UpdatePassword(UpdatePasswordDto updatePassword, string customerId);


    }
}
