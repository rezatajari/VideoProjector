namespace VideoProjector.DTOs.Order
{
    public class OrderGetDto
    {
        public int OrderId { get; set; }
        public string CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime? ShippingDate { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } // List of items
    }

}
