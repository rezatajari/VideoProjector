namespace VideoProjector.DTOs.OrderDetail
{
    public class OrderDetailAddDto
    {
        public int ProductId { get; set; }  // The ID of the product being ordered
        public int Quantity { get; set; }   // The quantity of the product being ordered
        public decimal Price { get; set; }  // The price of the product at the time of the order
    }

}
