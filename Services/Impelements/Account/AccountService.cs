using Microsoft.AspNetCore.Identity;
using System.Net;
using VideoProjector.Common;
using VideoProjector.Models;
using VideoProjector.Services.Interfaces;
using VideoProjector.DTOs.Account;

namespace VideoProjector.Services.Impelements.Account
{
    public class AccountService(UserManager<Customer> userManager, ILogger<AccountService> logger,
                   EmailConfirmationService emailConfirmationService, JwtTokenService jwtToken) : IAccountService
    {

        public async Task<ResponseCenter<string>> Login(LoginDto loginDto)
        {
            try
            {
                // Find the customer by email
                var customer = await userManager.FindByEmailAsync(loginDto.Email);

                // Check if the customer exists and the password is correct
                if (customer == null || !await userManager.CheckPasswordAsync(customer, loginDto.Password))
                {
                    logger.LogWarning("Failed login attempt for email: {Email}", loginDto.Email);
                    return ResponseCenter.CreateErrorResponse<string>(
                        message: "Invalid email or password.",
                        errorCode: "INVALID_CREDENTIALS");
                }

                // Check if email is confirmed
                if (!await userManager.IsEmailConfirmedAsync(customer))
                {
                    logger.LogWarning("Login failed: Email not confirmed for user {Email}", customer.Email);
                    return ResponseCenter.CreateErrorResponse<string>(
                        message: "Please confirm your email before logging in.",
                        errorCode: "EMAIL_NOT_CONFIRMED");
                }

                // Generate a JWT token for the customer
                var token = jwtToken.GenerateToken(customer.Id, customer.Email);

                logger.LogInformation("Customer logged in: {Email}", loginDto.Email);
                return ResponseCenter.CreateSuccessResponse(data: token, message: "Logged");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, message: "An error occurred while logging in.");
                return ResponseCenter.CreateErrorResponse<string>(
                    message: "An error occurred while logging in.",
                    errorCode: "LOGIN_ERROR");
            }
        }

        /// <summary>
        /// Registers a new customer with the provided registration details.
        /// </summary>
        /// <param name="registerDto">The registration details of the new customer.</param>
        /// <returns>A response indicating the success or failure of the registration process.</returns>
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

                if (!resultRegistration.Succeeded)
                {
                    logger.LogWarning("Failed to create a new customer: {Email}", registerDto.Email);

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


        private async Task<ResponseCenter<string>> SendEmailConfirmation(Customer customer)
        {
            try
            {
                // Generate the token for email confirmation
                var token = await userManager.GenerateEmailConfirmationTokenAsync(customer);
                var encodedToken = WebUtility.UrlEncode(token);

                // Construct the confirmation link
                const string frontendUrl = "http://localhost:61028";
                var confirmationLink = $"{frontendUrl}/confirm-email?customerId={customer.Id}&token={encodedToken}";

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

        /// <summary>
        /// Confirmation email 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ResponseCenter<string>> ConfirmEmail(string customerId, string token)
        {
            try
            {
                var customer = await userManager.FindByIdAsync(customerId);
                if (customer == null)
                    return ResponseCenter.CreateErrorResponse<string>(
                        message: "Customer not found",
                        errorCode: "NOT_FOUND");

                // Confirmation email
                var decodedToken = WebUtility.UrlDecode(token);
                var result = await userManager.ConfirmEmailAsync(customer, decodedToken);
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
