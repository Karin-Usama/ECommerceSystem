using Microsoft.AspNetCore.Http;

namespace ECommerceSystem.BLL
{
    public class CategoryCreateDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}
