using System.ComponentModel.DataAnnotations;

namespace VideoProjector.DTOs
{
    public class UpdatePasswordDto
    {
        [Required(ErrorMessage = "Current Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public required string CurrentPassword{ get; set; }

        [Required(ErrorMessage = "New Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public required string NewPassword { get; set; }
    }
}
