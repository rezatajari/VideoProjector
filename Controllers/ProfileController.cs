using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoProjector.Common;
using VideoProjector.DTOs;
using VideoProjector.Services.Impelements;

namespace VideoProjector.Controllers
{
    [Route(template: "api/profile")]
    [ApiController]
    [Authorize]
    public class ProfileController(ProfileService profileService) : ControllerBase
    {

        [HttpGet(template: "details")]
        public async Task<IActionResult> GetCustomerProfile()
        {
            try
            {
                // Extract customer ID from claims
                var customerId = User.FindFirst(type: ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(customerId))
                    return Unauthorized("Invalid token: Customer ID missing");

                var result = await profileService.GetCustomerProfileById(customerId);

                if (result.Status == "Error")
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(error: "An error occurred while processing your request.");
            }
        }

        [HttpPatch(template: "edit")]
        public async Task<IActionResult> Edit([FromBody] EditDto editDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseCenter.CreateErrorResponse<EditDto>(
                    message: "Validation failed",
                    errorCode: "VALIDATION_ERROR",
                    validationErrors: ModelState.Values.SelectMany(v => v.Errors).
                        Select(e => e.ErrorMessage).
                        ToList()));

            var customerId = User.FindFirst(type: ClaimTypes.NameIdentifier)?.Value;
            var result = await profileService.EditProfile(editDto, customerId!);

            if (result.Status == "Error")
                return BadRequest(result);
            return Ok(result);
        }


        // Change password endpoint
    }
}
