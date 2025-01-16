using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VideoProjector.Controllers
{
    [Route(template:"api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        // Summary of APIs for Order Controller:
        // List of Orders: Endpoint to retrieve a customer's order history.
        // Save Order: Endpoint to create and save a new order when a customer makes a purchase.
        // Check Order Status: Endpoint to check the current status(pending, complete, cancelled) of a specific order.
        // Show Order Details: Endpoint to show detailed information about the products in an order.
        // Cancel Order: Optional endpoint to allow customers to cancel an order before it is completed.
        // Update Order: Optional endpoint to update order details (like shipping info or quantities).
    }
}
