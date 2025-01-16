using System.ComponentModel.DataAnnotations;

namespace VideoProjector.DTOs.Profile
{
    public class ProfileDto
    {
        [MaxLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
        public string? Username { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
