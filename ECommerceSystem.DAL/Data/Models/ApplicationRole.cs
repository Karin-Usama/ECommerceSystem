using Microsoft.AspNetCore.Identity;

namespace ECommerceSystem.DAL
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
    }
}
