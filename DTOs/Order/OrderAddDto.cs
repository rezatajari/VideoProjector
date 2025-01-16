using VideoProjector.DTOs.OrderDetail;

namespace VideoProjector.DTOs.Order
{
    public class OrderAddDto
    {
        public string CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public string ShippingAddress { get; set; }
        public List<OrderDetailAddDto> OrderDetails { get; set; } // List of items in the order
    }

}
