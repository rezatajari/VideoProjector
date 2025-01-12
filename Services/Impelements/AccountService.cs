using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VideoProjector.Common;
using VideoProjector.DTOs;
using VideoProjector.Models;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Services.Impelements
{
    public class AccountService(UserManager<Customer> userManager, ILogger<AccountService> logger, JwtTokenService jwtToken) : IAccountService
    {

        // Method to handle user login
        public async Task<ResponseCenter<string>> Login(LoginDto loginDto)
        {
            try
            {
                // Find the user by email
                var customer = await userManager.FindByEmailAsync(loginDto.Email);

                // Check if the user exists and the password is correct
                if (customer == null || !await userManager.CheckPasswordAsync(customer, loginDto.Password))
                {
                    // Log a warning for a failed login attempt
                    logger.LogWarning("Failed login attempt for email: {Email}", loginDto.Email);

                    // Return an error response for invalid credentials
                    return ResponseCenter.CreateErrorResponse<string>(
                        message: "Invalid email or password.",
                        errorCode: "INVALID_CREDENTIALS");
                }

                // Generate a JWT token for the user
                var token = jwtToken.GenerateToken(customer.Id, customer.Email);

                // Log the successful login
                logger.LogInformation("User logged in: {Email}", loginDto.Email);

                // Return a success response with the token
                return ResponseCenter.CreateSuccessResponse(data: token, message: "Logged");
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex, message: "An error occurred while logging in.");

                // Return an error response for the exception
                return ResponseCenter.CreateErrorResponse<string>(
                    message: "An error occurred while logging in.",
                    errorCode: "LOGIN_ERROR");
            }
        }
    }
}
