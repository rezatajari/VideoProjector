﻿namespace Front.DTOs.Category
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int TotalProducts { get; set; }
        public string? ImageUrl { get; set; }
    }
}
