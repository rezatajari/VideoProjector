
namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string Description { get; set; }

        public string MainImage { get; set; }

        public int CategoryId { get; set; }


        // Relationships
        public virtual Category Category { get; set; }
    }
}

