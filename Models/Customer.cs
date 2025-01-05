using Microsoft.AspNetCore.Identity;

namespace VideoProjector.Models
{
    public class Customer : IdentityUser
    {
        // Additional properties specific to Customer
        public string? Address { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsDeleted { get; set; }  // Optional for soft delete
        public string? ProfilePicture { get; set; }  // Optional for profile image URL or path
        public string? Gender { get; set; }  // Optional for gender

        public virtual ICollection<ShoppingCart>? ShoppingCarts { get; set; } // Navigation property for Shopping Carts.
        public virtual ICollection<Order>? Orders { get; set; } // Navigation property for Orders.
    }
}
