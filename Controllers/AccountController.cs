using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoProjector.Common;
using VideoProjector.DTOs.Account;
using VideoProjector.Services.Impelements.Account;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Controllers
{
    [Route(template: "api/account")]
    [ApiController]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        /// <summary>
        /// Handles the login request.
        /// </summary>
        /// <param name="loginDto">The login data transfer object containing user credentials.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the login operation.
        /// Returns a <see cref="BadRequestObjectResult"/> if the model state is invalid or if the login fails.
        /// Returns an <see cref="OkObjectResult"/> if the login is successful.
        /// </returns>
        /// <remarks>
        /// This method validates the model state and calls the login method in the AccountService.
        /// </remarks>
        [HttpPost(template: "login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await accountService.Login(loginDto);

            if (result.Status == "Error")
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Handles the registration request.
        /// </summary>
        /// <param name="registerDto">The registration data transfer object containing user details.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the registration operation.
        /// </returns>
        /// <remarks>
        /// This method validates the model state and calls the register method in the AccountService.
        /// </remarks>
        [HttpPost(template: "register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await accountService.Register(registerDto);

            if (result.Status == "Error")
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Handles the email confirmation request.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <param name="token">The email confirmation token.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the email confirmation operation.
        /// </returns>
        /// <remarks>
        /// This method validates the input parameters and calls the confirm email method in the AccountService.
        /// </remarks>
        [HttpPost(template: "confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string customerId, string token)
        {
            // Validate the input parameters
            if (string.IsNullOrEmpty(customerId) || string.IsNullOrEmpty(token))
                // Return a bad request response with an error message
                return BadRequest(ResponseCenter.CreateErrorResponse<string>(
                    message: "Invalid email confirmation request.",
                    errorCode: "NULL_OR_EMPTY"));

            // Call the confirm email method in the AccountService
            var result = await accountService.ConfirmEmail(customerId, token);

            // Check if the email confirmation was unsuccessful
            if (result.Status == "Error")
                // Return a bad request response with the error details
                return BadRequest(result);

            // Return an OK response with the success details
            return Ok(result);
        }
    }
}
