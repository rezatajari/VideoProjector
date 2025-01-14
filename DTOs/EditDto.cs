using System.ComponentModel.DataAnnotations;

namespace VideoProjector.DTOs
{
    public class EditDto
    {
        [MaxLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }
        public string? Address { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public string? CurrentProfilePicturePath { get; set; } // Existing file path
    }
}
