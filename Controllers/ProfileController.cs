using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoProjector.Common;
using VideoProjector.DTOs.Profile;
using VideoProjector.Services.Impelements;
using VideoProjector.Services.Interfaces;

namespace VideoProjector.Controllers
{
    [Route(template: "api/profile")]
    [ApiController]
    [Authorize]
    public class ProfileController(IProfileService profileService) : ControllerBase
    {

        [HttpGet(template: "details")]
        public async Task<IActionResult> GetCustomerProfile()
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

        [HttpGet(template: "edit")]
        public async Task<IActionResult> Edit()
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await profileService.GetEditProfile(customerId);

            if (result.Status == "Error")
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPatch(template: "edit")]
        public async Task<IActionResult> Edit([FromForm] EditDto editDto)
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

        [HttpPatch(template: "update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseCenter.CreateErrorResponse<UpdatePasswordDto>(
                    message: "Validation failed",
                    errorCode: "VALIDATION_ERROR",
                    validationErrors: ModelState.Values.SelectMany(v => v.Errors).
                        Select(e => e.ErrorMessage).
                        ToList()));

            var customerId = User.FindFirst(type: ClaimTypes.NameIdentifier)?.Value;

            var result = await profileService.UpdatePassword(updatePassword, customerId!);

            if (!result.Data.Succeeded)
                return BadRequest(result);
            return Ok(result);
        }

    }
}
