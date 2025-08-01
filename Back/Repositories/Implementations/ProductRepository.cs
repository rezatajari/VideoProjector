﻿using Back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using VideoProjector.Data;
using VideoProjector.DTOs.Product;
using VideoProjector.Models;

namespace Back.Repositories.Implementations
{
    public class ProductRepository(VpDatabase database) : IProductRepository
    {
        public async Task<List<Product>> GetlProductsAsync()
        {
            return await database.Products
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Product> Details(int productId)
        {
            var product = await database.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == productId);
            return product;

        }

        public async Task<List<Product>> GetProductSearch(ProductSearchDto searchDto)
        {
            var query = database.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchDto.SearchTerm))
                query = query.Where(p => p.Name.Contains(searchDto.SearchTerm));
            if (searchDto.MinPrice.HasValue)
                query = query.Where(p => p.Price >= searchDto.MinPrice.Value);
            if (searchDto.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= searchDto.MaxPrice.Value);

            // Execute the query
            return await query.ToListAsync();
        }
    }
}
