namespace VideoProjector.DTOs.Category
{
    public record CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int ProductCount { get; set; }
        public string? ImageUrl { get; set; }
    }
}
