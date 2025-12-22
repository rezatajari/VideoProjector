using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Category
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }  // Name of the category (e.g., "Projectors")
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
