namespace Shared.Models;

public class Order : BaseEntity
{
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public bool IsRental { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
}