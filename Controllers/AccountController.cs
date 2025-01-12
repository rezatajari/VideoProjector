using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoProjector.Common;
using VideoProjector.DTOs;
using VideoProjector.Services.Impelements;

namespace VideoProjector.Controllers
{
    [Route(template: "api/account")]
    [ApiController]
    public class AccountController(AccountService accountService) : ControllerBase
    {
        // Endpoint for user login
        [Authorize]
        [HttpPost(template: "login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // Validate the model state
            if (!ModelState.IsValid)
            {
                // Return a bad request response with validation errors
                return BadRequest(ResponseCenter.CreateErrorResponse<LoginDto>(
                  message: "Validation failed",
                  errorCode: "VALIDATION_ERROR",
                  validationErrors: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()));
            }

            // Call the login method in the AccountService
            var result = await accountService.Login(loginDto);

            // Check if the login was unsuccessful
            if (result.Status == "Error")
                // Return a bad request response with the error details
                return BadRequest(result);

            // Return an OK response with the success details
            return Ok(result);
        }


        // Endpoint for user registration
        [HttpPost(template: "register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseCenter.CreateErrorResponse<RegisterDto>(
                    message: "Validation failed",
                    errorCode: "VALIDATION_ERROR",
                    validationErrors: ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList()));

            var result = await accountService.Register(registerDto);

            if (result.Status == "Error")
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost(template: "confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string customerId, string token)
        {
            if (string.IsNullOrEmpty(customerId) || string.IsNullOrEmpty(token))
                return BadRequest(ResponseCenter.CreateErrorResponse<string>(
                    message: "Invalid email confirmation request.",
                    errorCode: "NULL_OR_EMPTY"));

            var result = await accountService.ConfirmEmail(customerId, token);

            if (result.Status == "Error")
                return BadRequest(result);
            return Ok(result);
        }
    }
}
