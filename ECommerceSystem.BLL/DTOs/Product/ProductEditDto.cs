using Microsoft.AspNetCore.Http;

namespace ECommerceSystem.BLL
{
    public class ProductEditDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
