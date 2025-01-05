namespace VideoProjector.Models
{
    public class Category
    {
        public int CategoryId { get; set; }  // Primary Key
        public string? Name { get; set; }  // Name of the category (e.g., "Projectors")
        public string? Description { get; set; }  // Description of the category

        // Navigation property to Products (One-to-Many relationship: one category can have many products)
        public virtual ICollection<Product>? Products { get; set; }
    }
}
