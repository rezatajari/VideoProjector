using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Azure.Identity;
using VideoProjector.Common;
using VideoProjector.DTOs;
using VideoProjector.Models;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Services.Impelements
{
    public class AccountService(UserManager<Customer> userManager, ILogger<AccountService> logger,
                   EmailConfirmationService emailConfirmationService, JwtTokenService jwtToken) : IAccountService
    {

        // Method to handle customer login
        public async Task<ResponseCenter<string>> Login(LoginDto loginDto)
        {
            try
            {
                // Find the customer by email
                var customer = await userManager.FindByEmailAsync(loginDto.Email);

                // Check if the customer exists and the password is correct
                if (customer == null || !await userManager.CheckPasswordAsync(customer, loginDto.Password))
                {
                    // Log a warning for a failed login attempt
                    logger.LogWarning("Failed login attempt for email: {Email}", loginDto.Email);

                    // Return an error response for invalid credentials
                    return ResponseCenter.CreateErrorResponse<string>(
                        message: "Invalid email or password.",
                        errorCode: "INVALID_CREDENTIALS");
                }

                // Generate a JWT token for the customer
                var token = jwtToken.GenerateToken(customer.Id, customer.Email);

                // Log the successful login
                logger.LogInformation("Customer logged in: {Email}", loginDto.Email);

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

        // Method to handle customer registration
        public async Task<ResponseCenter<string>> Register(RegisterDto registerDto)
        {
            try
            {
                // Map RegisterDto to Customer
                var newCustomer = new Customer
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                    Address = registerDto.Address,
                    Gender = registerDto.Gender,
                    RegistrationDate = DateTime.UtcNow
                };

                // Create the user
                var resultRegistration = await userManager.CreateAsync(newCustomer, registerDto.Password);

                // Check if the user creation was successful
                if (!resultRegistration.Succeeded)
                {
                    // Log a warning for a failed registration attempt
                    logger.LogWarning("Failed to create a new customer: {Email}", registerDto.Email);

                    // Return an error response for failed registration
                    return ResponseCenter.CreateErrorResponse<string>(
                        message: "Failed to create a new customer.",
                        errorCode: "CREATE_CUSTOMER_ERROR",
                        validationErrors: resultRegistration.Errors.Select(e => e.Description).ToList());
                }

                // Send email confirmation
                var emailSent = await SendEmailConfirmation(newCustomer);
                if (emailSent.Status == "Error")
                    return ResponseCenter.CreateErrorResponse<string>(
                        message: emailSent.Message,
                        errorCode: emailSent.ErrorCode);

                // Log the successful registration
                logger.LogInformation("Customer registered successfully: {Email}", registerDto.Email);

                // Return a success response
                return ResponseCenter.CreateSuccessResponse("Customer registered successfully. Please check your email to confirm your account.", message: "REGISTRATION_SUCCESS");
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex, "An error occurred while registering a new customer.");

                // Return an error response for the exception
                return ResponseCenter.CreateErrorResponse<string>(
                    message: "An error occurred while registering a new customer.",
                    errorCode: "REGISTER_ERROR");
            }
        }


        // Method to send email confirmation
        private async Task<ResponseCenter<string>> SendEmailConfirmation(Customer customer)
        {
            try
            {
                // Generate the token for email confirmation
                var token = await userManager.GenerateEmailConfirmationTokenAsync(customer);

                // Construct the confirmation link
                var frontendUrl = "http://localhost:5098";
                var confirmationLink = $"{frontendUrl}/confirm-email?customerId={customer.Id}&token={WebUtility.UrlEncode(token)}";

                await emailConfirmationService.SendConfirmationEmailAsync(customer.Email, "Confirm your email", $"Please confirm your email by clicking <a href='{confirmationLink}'>here</a>.");

                return ResponseCenter.CreateSuccessResponse(data: string.Empty);
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to send email confirmation to: {Email}", customer.Email);
                return ResponseCenter.CreateErrorResponse<string>(
                    message: "Customer created, but failed to send confirmation email.",
                    errorCode: "EMAIL_CONFIRMATION_ERROR");
            }
        }

        public async Task<ResponseCenter<string>> ConfirmEmail(string customerId, string token)
        {
            try
            {
                // Find customer
                var customer = await userManager.FindByIdAsync(customerId);
                if (customer == null)
                    return ResponseCenter.CreateErrorResponse<string>(
                        message: "Customer not found",
                        errorCode: "NOT_FOUND");

                // Confirmation email
                var result = await userManager.ConfirmEmailAsync(customer, token);
                if (!result.Succeeded)
                    return ResponseCenter.CreateErrorResponse<string>(
                        message: "Failed to confirm email",
                        errorCode: "CONFIRMATION_ERROR",
                        validationErrors: result.Errors.Select(e => e.Description).ToList());

                // Successful confirmed
                logger.LogInformation("Email confirmation is successful for the customerId {CustomerId}", customerId);
                return ResponseCenter.CreateSuccessResponse(
                    data: "Email confirmed successfully.",
                    message: "EMAIL_CONFIRMED");
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to confirmation customerId: {CustomerId}", customerId);
                return ResponseCenter.CreateErrorResponse<string>(
                    message: "Confirmation email failed",
                    errorCode: "CONFIRMATION_ERROR");

            }
        }
    }
}
