using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(VideoProjectorDbContext context) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await context.Products
            .Where(p => !p.IsDeleted)
            .ToListAsync();

        return Ok(products);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await context.Products
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        if (product == null)
        {
            return NotFound("Product not found or has been removed.");
        }

        return Ok(product);
    }


    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest("ID mismatch.");
        }

        // چک کردن اینکه آیا محصول هنوز وجود دارد و حذف نشده است
        var existingProduct = await context.Products
            .AnyAsync(p => p.Id == id && !p.IsDeleted);

        if (!existingProduct)
        {
            return NotFound("Product not found or has been removed.");
        }

        context.Entry(product).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }

        return NoContent(); 
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);

        if (product == null || product.IsDeleted)
        {
            return NotFound("Product not found or already deleted.");
        }

        product.IsDeleted = true;

        await context.SaveChangesAsync();

        return NoContent();
    }
}
