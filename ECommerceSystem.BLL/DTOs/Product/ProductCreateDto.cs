using Microsoft.AspNetCore.Http;

namespace ECommerceSystem.BLL
{
    public class ProductCreateDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
