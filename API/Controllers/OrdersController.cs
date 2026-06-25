using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(VideoProjectorDbContext context) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(Order order)
    {
        var product = await context.Products
            .FirstOrDefaultAsync(p => p.Id == order.ProductId && !p.IsDeleted);

        if (product == null)
        {
            return NotFound("محصول مورد نظر یافت نشد یا از سیستم حذف شده است.");
        }

        if (order.IsRental)
        {
            if (order.StartDate == null || order.EndDate == null)
            {
                return BadRequest("برای سفارشات اجاره‌ای، تعیین تاریخ شروع و پایان الزامی است.");
            }

            if (order.StartDate.Value.Date < DateTime.Now.Date)
            {
                return BadRequest("تاریخ شروع اجاره نمی‌تواند در گذشته باشد.");
            }

            if (order.EndDate.Value < order.StartDate.Value)
            {
                return BadRequest("تاریخ پایان اجاره نمی‌تواند قبل از تاریخ شروع باشد.");
            }

            if (product.QuantityForRental < order.Quantity)
            {
                return BadRequest($"موجودی کافی برای اجاره این کالا وجود ندارد. موجودی فعلی: {product.QuantityForRental}");
            }

            int totalDays = (order.EndDate.Value.Date - order.StartDate.Value.Date).Days;
            if (totalDays == 0) totalDays = 1;

            order.TotalPrice = totalDays * order.Quantity * (product.RentalPricePerDay ?? 0);
        }
        else
        {
            if (product.SalePrice == null)
            {
                return BadRequest("این محصول برای فروش نقدی در دسترس نیست.");
            }

            if (product.QuantityForSale < order.Quantity)
            {
                return BadRequest($"موجودی انبار کافی نیست. موجودی فعلی: {product.QuantityForSale}");
            }

            product.QuantityForSale -= order.Quantity;

            order.TotalPrice = order.Quantity * product.SalePrice.Value;
        }

        context.Orders.Add(order);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrderById(int id)
    {
        var order = await context.Orders.FindAsync(id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        var orders = await context.Orders
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return Ok(orders);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatus newStatus)
    {
        var order = await context.Orders.FindAsync(id);

        if (order == null)
        {
            return NotFound("سفارش مورد نظر یافت نشد.");
        }

        if (order.IsRental == false && newStatus == OrderStatus.Canceled && order.Status != OrderStatus.Canceled)
        {
            // اگر فروش قطعی لغو شد، موجودی کالا باید به انبار فروش برگردد
            var product = await context.Products.FindAsync(order.ProductId);
            if (product != null)
            {
                product.QuantityForSale += order.Quantity;
            }
        }

        order.Status = newStatus;

        await context.SaveChangesAsync();

        return NoContent();
    }
}