﻿using System.ComponentModel.DataAnnotations;

namespace VideoProjectorFront.DTOs.Account
{
    public class RegisterDto
    {
        [MaxLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [RegularExpression(pattern: "^(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).{8,}$")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public  string? ConfirmPassword { get; set; }

        [MaxLength(250, ErrorMessage = "Address cannot be longer than 250 characters.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
