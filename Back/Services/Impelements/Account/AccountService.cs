using Microsoft.AspNetCore.Identity;
using System.Net;
using VideoProjector.Common;
using VideoProjector.Models;
using VideoProjector.Services.Interfaces;
using VideoProjector.DTOs.Account;

namespace VideoProjector.Services.Impelements.Account
{
    /// <summary>
    /// Service of account customers
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="logger"></param>
    /// <param name="emailConfirmationService"></param>
    /// <param name="jwtToken"></param>
    public class AccountService(UserManager<Customer> userManager, ILogger<AccountService> logger,
                   EmailConfirmationService emailConfirmationService, JwtTokenService jwtToken) : IAccountService
    {

        /// <summary>
        /// Login service layer
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        public async Task<GeneralResponse<string>> Login(LoginDto loginDto)
        {
            try
            {
                var customer = await userManager.FindByEmailAsync(loginDto.Email);

                // Check if the customer exists and the password is correct
                if (customer == null || !await userManager.CheckPasswordAsync(customer, loginDto.Password))
                {
                    logger.LogWarning("Failed login attempt for email: {Email}", loginDto.Email);
                    return GeneralResponse<string>.Failure(message: "Invalid email or password.");
                }

                // Check if email is confirmed
                if (!await userManager.IsEmailConfirmedAsync(customer))
                {
                    logger.LogWarning("Login failed: Email not confirmed for user {Email}", customer.Email);
                    return GeneralResponse<string>.Failure(message: "Please confirm your email before logging in.");
                }

                // Generate a JWT token for the customer
                var token = jwtToken.GenerateToken(customer.Id, customer.Email);

                logger.LogInformation("Customer logged in: {Email}", loginDto.Email);
                return GeneralResponse<string>.Success(data: token, message: "Logged");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, message: "An error occurred while logging in.");
                return GeneralResponse<string>.Failure(message: "An error occurred while logging in.");
            }
        }

        /// <summary>
        /// Registers a new customer with the provided registration details.
        /// </summary>
        /// <param name="registerDto">The registration details of the new customer.</param>
        /// <returns>A response indicating the success or failure of the registration process.</returns>
        public async Task<GeneralResponse<string>> Register(RegisterDto registerDto)
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

                    return GeneralResponse<string>.Failure(message: "Failed to create a new customer.");
                }

                // Send email confirmation
                var emailSent = await SendEmailConfirmation(newCustomer);
                if (!emailSent.IsSuccess )
                    return GeneralResponse<string>.Failure(message: emailSent.Message);

                // Log the successful registration
                logger.LogInformation("Customer registered successfully: {Email}", registerDto.Email);

                // Return a success response
                return GeneralResponse<string>.Success("Customer registered successfully. Please check your email to confirm your account.", message: "REGISTRATION_SUCCESS");
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex, "An error occurred while registering a new customer.");

                // Return an error response for the exception
                return GeneralResponse<string>.Failure(message: "An error occurred while registering a new customer.");
            }
        }


        private async Task<GeneralResponse<string>> SendEmailConfirmation(Customer customer)
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

                return GeneralResponse<string>.Success(data: string.Empty);
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to send email confirmation to: {Email}", customer.Email);
                return GeneralResponse<string>.Failure(message: "Customer created, but failed to send confirmation email.");
            }
        }

        /// <summary>
        /// Confirmation email 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<GeneralResponse<string>> ConfirmEmail(string customerId, string token)
        {
            try
            {
                var customer = await userManager.FindByIdAsync(customerId);
                if (customer == null)
                    return GeneralResponse<string>.Failure(message: "Customer not found");

                // Confirmation email
                var decodedToken = WebUtility.UrlDecode(token);
                var result = await userManager.ConfirmEmailAsync(customer, decodedToken);
                if (!result.Succeeded)
                    return GeneralResponse<string>.Failure(message: "Failed to confirm email");

                // Successful confirmed
                logger.LogInformation("Email confirmation is successful for the customerId {CustomerId}", customerId);
                return GeneralResponse<string>.Success(data: "Email confirmed successfully.",
                    message: "EMAIL_CONFIRMED");
                   
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to confirmation customerId: {CustomerId}", customerId);
                return GeneralResponse<string>.Failure(message: "Confirmation email failed");

            }
        }
    }
}
