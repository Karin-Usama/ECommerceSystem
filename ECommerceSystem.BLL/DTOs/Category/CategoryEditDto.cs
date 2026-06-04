using Microsoft.AspNetCore.Http;

namespace ECommerceSystem.BLL
{
    public class CategoryEditDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}
