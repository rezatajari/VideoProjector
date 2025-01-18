namespace VideoProjector.Models
{
    public class Order
    {
        public int OrderId { get; set; }  // Primary Key
        public required string CustomerId { get; set; }   // Foreign Key to Customer (Identity User)
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public required string OrderStatus { get; set; }// E.g., Pending, Completed, Cancelled
        public required string ShippingAddress { get; set; }
        public DateTime? ShippingDate { get; set; }  // Optional, when the order is shipped

        // Navigation property to link to the Customer
        public virtual  Customer Customer { get; set; }

        // Navigation property for order details (items in the order)
        public virtual  ICollection<OrderDetail> OrderDetails { get; set; }
    }

}
