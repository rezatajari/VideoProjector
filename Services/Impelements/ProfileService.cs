using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

            var getProfilePicturePath = await GenerateProfilePicture(editDto.ProfilePicture);

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

        private async Task<ResponseCenter<string>> GenerateProfilePicture(IFormFile editDtoProfilePicture)
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
            return ResponseCenter.CreateSuccessResponse(data: fileUrl);

        }
    }
}
