using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.CompilerServices;
using VideoProjector.Common;
using VideoProjector.DTOs.Profile;
using VideoProjector.Models;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Services.Impelements
{
    public class ProfileService(UserManager<Customer> userManager, ILogger<ProfileService> logger) : IProfileService
    {
        public async Task<GeneralResponse<ProfileDto>> GetCustomerProfileById(string customerId)
        {

            var customer = await userManager.FindByIdAsync(customerId);

            if (customer == null)
            {
                logger.LogWarning("Customer is not found and null by this ID: {ID}", customerId);
                return GeneralResponse<ProfileDto>.Failure(message: "Customer is null");
            }

            var profileDto = new ProfileDto
            {
                Username = customer.UserName,
                RegistrationDate = customer.RegistrationDate,
                ProfilePicture = customer.ProfilePicture
            };

            return GeneralResponse<ProfileDto>.Success(data: profileDto);

        }

        public async Task<GeneralResponse<EditDto>> GetEditProfile(string customerId)
        {
            var customer = await userManager.FindByIdAsync(customerId);
            if (customer == null)
            {
                logger.LogWarning("Customer by this ID {ID} is not found", customerId);
                return GeneralResponse<EditDto>.Failure(message: "Customer is not found");
            }

            var editProfile = new EditDto
            {
                Username = customer.UserName,
                Email = customer.Email!,
                Address = customer.Address,
                CurrentProfilePicturePath = customer.ProfilePicture
            };

            logger.LogInformation("Get edit profile successful for this ID: {ID}", customerId);
            return GeneralResponse<EditDto>.Success(data: editProfile);
        }

        public async Task<GeneralResponse<IdentityResult>> EditProfile(EditDto editDto, string customerId)
        {
            var customer = await userManager.FindByIdAsync(customerId);
            if (customer == null)
            {
                logger.LogWarning("Customer is null by this ID: {ID}", customerId);
                return GeneralResponse<IdentityResult>.Failure(message: "Customer is null");
            }

            if (editDto.Email != customer.Email)
            {
                logger.LogWarning("New email is not confirmed: {Email}", editDto.Email);
                return GeneralResponse<IdentityResult>.Failure(message: "New email required to confirmed");
            }

            var getProfilePicturePath = await GenerateProfilePicture(editDto.ProfilePicture, editDto.CurrentProfilePicturePath);

            customer.UserName = editDto.Username;
            customer.Email = editDto.Email;
            customer.Address = editDto.Address;
            customer.ProfilePicture = getProfilePicturePath.Data;

            var result = await userManager.UpdateAsync(customer);
            if (!result.Succeeded)
            {
                logger.LogWarning("Update operation is problem for this ID: {ID}", customer.Id);
                return GeneralResponse<IdentityResult>.Failure(message: "Failed update profile");
            }

            logger.LogInformation("Profiled is successful to update for this ID: {ID}", customerId);
            return GeneralResponse<IdentityResult>.Success(data: result, message: "Update successful");
        }

        public async Task<GeneralResponse<IdentityResult>> UpdatePassword(UpdatePasswordDto updatePassword, string customerId)
        {
            var customer = await userManager.FindByIdAsync(customerId);
            if (customer == null)
            {
                logger.LogWarning("Customer is not fined by this ID: {ID}", customerId);
                GeneralResponse<IdentityResult>.Failure(message: "Customer is not found");
            }

            var result = await userManager.ChangePasswordAsync(customer!, updatePassword.CurrentPassword,
                updatePassword.NewPassword);

            if (!result.Succeeded)
            {
                logger.LogWarning("Password does not update for this customer ID: {ID}", customerId);
                return GeneralResponse<IdentityResult>.Failure(message: "Password does not update");
            }

            logger.LogInformation("Password update successfully for this ID: {ID}", customerId);
            return GeneralResponse<IdentityResult>.Success(data: result, message: "Password updated");
        }

        private async Task<GeneralResponse<string>> GenerateProfilePicture(IFormFile editDtoProfilePicture, string currentProfilePicture)
        {
            if (editDtoProfilePicture is not { Length: > 0 })
                return GeneralResponse<string>.Failure(message: "No file uploaded.");

            // Check file extension (optional)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(editDtoProfilePicture.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                logger.LogWarning("Invalid format picture");
                return GeneralResponse<string>.Failure(message: "Invalid file format. Please upload a .jpg, .jpeg, or .png image.");
            }

            // Generate a unique filename and save the file
            var fileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);

            // Save the file to disk
            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await editDtoProfilePicture.CopyToAsync(fileStream);
            logger.LogInformation("Save picture is successful");

            var fileUrl = $"/uploads/{fileName}";

            if (string.IsNullOrEmpty(currentProfilePicture)) return GeneralResponse<string>.Success(data: fileUrl);

            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), currentProfilePicture.TrimStart('/'));
            if (File.Exists(oldFilePath))
            {
                File.Delete(oldFilePath);
            }

            return GeneralResponse<string>.Success(data: fileUrl);
        }
    }
}
