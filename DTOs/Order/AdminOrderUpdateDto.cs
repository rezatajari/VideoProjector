namespace VideoProjector.DTOs.Order
{
    public class AdminOrderUpdateDto
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; }  // E.g., "Pending", "Shipped", "Cancelled"
        public DateTime? ShippingDate { get; set; }
    }

}
