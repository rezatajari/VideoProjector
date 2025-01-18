namespace VideoProjector.DTOs.Order
{
    public class CustomerOrderUpdateDto
    {
        public int OrderId { get; set; }
        public required string ShippingAddress { get; set; }
    }

}
