namespace VideoProjector.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }  // Primary Key
        public int OrderId { get; set; }  // Foreign Key to Order
        public int ProductId { get; set; }  // Foreign Key to Product
        public int Quantity { get; set; }
        public decimal Price { get; set; }  // Price at the time of order

        // Navigation property to Order
        public virtual Order? Order { get; set; }

        // Navigation property to Product
        public virtual Product? Product { get; set; }
    }

}
