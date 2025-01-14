using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.CompilerServices;
using VideoProjector.Common;
using VideoProjector.DTOs;
using VideoProjector.Models;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Services.Impelements
{
    public class ProfileService(UserManager<Customer> userManager, ILogger<ProfileService> logger) : IProfileService
    {
        public async Task<ResponseCenter<ProfileDto>> GetCustomerProfileById(string customerId)
        {
            Customer? customer;
            try
            {
                customer = await userManager.FindByIdAsync(customerId);
            }
            catch (Exception ex)
            {
                logger.LogError("Find customer is interrupt for this ID: {ID}", customerId);
                return ResponseCenter.CreateErrorResponse<ProfileDto>(
                    message: "Failed find operation find customer",
                    errorCode: "NOT_FOUND");
            }

            if (customer == null)
            {
                logger.LogWarning("Customer is not found and null by this ID: {ID}", customerId);
                return ResponseCenter.CreateErrorResponse<ProfileDto>(
                    message: "Customer is null",
                    errorCode: "NULL");
            }

            var profileDto = new ProfileDto
            {
                Username = customer.UserName,
                RegistrationDate = customer.RegistrationDate,
                ProfilePicture = customer.ProfilePicture
            };

            return ResponseCenter.CreateSuccessResponse(data: profileDto);

        }

        public async Task<ResponseCenter<EditDto>> GetEditProfile(string customerId)
        {
            try
            {
                var customer = await userManager.FindByIdAsync(customerId);
                if (customer == null)
                {
                    logger.LogWarning("Customer by this ID {ID} is not found", customerId);
                    return ResponseCenter.CreateErrorResponse<EditDto>(
                        message: "Customer is not found",
                        errorCode: "NULL");
                }

                var editProfile = new EditDto
                {
                    Username = customer.UserName,
                    Email = customer.Email!,
                    Address = customer.Address,
                    CurrentProfilePicturePath = customer.ProfilePicture
                };

                logger.LogInformation("Get edit profile successful for this ID: {ID}", customerId);
                return ResponseCenter.CreateSuccessResponse(data: editProfile);
            }
            catch (Exception ex)
            {
                logger.LogError("Get edit profile page error {error}", ex.Message);
                return ResponseCenter.CreateErrorResponse<EditDto>(
                    message: "Get edit page failed",
                    errorCode: "FAILED_OPERATION");
            }

        }

        public async Task<ResponseCenter<IdentityResult>> EditProfile(EditDto editDto, string customerId)
        {
            var customer = await userManager.FindByIdAsync(customerId);
            if (customer == null)
            {
                logger.LogWarning("Customer is null by this ID: {ID}", customerId);
                return ResponseCenter.CreateErrorResponse<IdentityResult>(
                    message: "Customer is null",
                    errorCode: "NULL");
            }

            if (editDto.Email != customer.Email)
            {
                logger.LogWarning("New email is not confirmed: {Email}", editDto.Email);
                return ResponseCenter.CreateErrorResponse<IdentityResult>(
                    message: "New email required to confirmed",
                    errorCode: "NOT_CONFIRMED");
            }

            var getProfilePicturePath = await GenerateProfilePicture(editDto.ProfilePicture,editDto.CurrentProfilePicturePath);
   
            customer.UserName = editDto.Username;
            customer.Email = editDto.Email;
            customer.Address = editDto.Address;
            customer.ProfilePicture = getProfilePicturePath.Data;

            var result = await userManager.UpdateAsync(customer);
            if (!result.Succeeded)
            {
                logger.LogWarning("Update operation is problem for this ID: {ID}", customer.Id);
                return ResponseCenter.CreateErrorResponse<IdentityResult>(
                    message: "Failed update profile",
                    errorCode: "FAILED_UPDATE");
            }

            logger.LogInformation("Profiled is successful to update for this ID: {ID}", customerId);
            return ResponseCenter.CreateSuccessResponse(data: result, message: "Update successful");
        }

        public async Task<ResponseCenter<IdentityResult>> UpdatePassword(UpdatePasswordDto updatePassword, string customerId)
        {
            try
            {
                var customer = await userManager.FindByIdAsync(customerId);
                if (customer == null)
                {
                    logger.LogWarning("Customer is not fined by this ID: {ID}", customerId);
                    ResponseCenter.CreateErrorResponse<IdentityResult>(
                        message: "Customer is not found",
                        errorCode: "NULL");
                }

                var result = await userManager.ChangePasswordAsync(customer!, updatePassword.CurrentPassword,
                    updatePassword.NewPassword);

                if (!result.Succeeded)
                {
                    logger.LogWarning("Password does not update for this customer ID: {ID}", customerId);
                    return ResponseCenter.CreateErrorResponse<IdentityResult>(
                        message: "Password does not update",
                        errorCode: "FAILED_UPDATE");
                }

                logger.LogInformation("Password update successfully for this ID: {ID}", customerId);
                return ResponseCenter.CreateSuccessResponse(data: result, message: "Password updated");
            }
            catch (Exception ex)
            {
                logger.LogError("Password operation is error: {error}", ex.Message);
                return ResponseCenter.CreateErrorResponse<IdentityResult>(
                    message: "Update password failed",
                    errorCode: "UPDATE_FAILED");
            }
        }

        private async Task<ResponseCenter<string>> GenerateProfilePicture(IFormFile editDtoProfilePicture,string currentProfilePicture)
        {
            if (editDtoProfilePicture is not { Length: > 0 })
                return ResponseCenter.CreateErrorResponse<string>(
                    message: "No file uploaded.",
                    errorCode: "NO_FILE");

            // Check file extension (optional)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(editDtoProfilePicture.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                logger.LogWarning("Invalid format picture");
                return ResponseCenter.CreateErrorResponse<string>(
                    message: "Invalid file format. Please upload a .jpg, .jpeg, or .png image.",
                    errorCode: "INVALID_FORMAT");
            }

            // Generate a unique filename and save the file
            var fileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);

            try
            {
                // Save the file to disk
                await using var fileStream = new FileStream(filePath, FileMode.Create);
                await editDtoProfilePicture.CopyToAsync(fileStream);
                logger.LogInformation("Save picture is successful");
            }
            catch (Exception ex)
            {
                logger.LogError("In save picture profile is error {ex}", ex);
                return ResponseCenter.CreateErrorResponse<string>(
                    message: "Save operation image failed",
                    errorCode: "NOT_SAVE");
            }

            var fileUrl = $"/uploads/{fileName}";

            if (!string.IsNullOrEmpty(currentProfilePicture))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), currentProfilePicture.TrimStart('/'));
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }

            return ResponseCenter.CreateSuccessResponse(data: fileUrl);

        }
    }
}
