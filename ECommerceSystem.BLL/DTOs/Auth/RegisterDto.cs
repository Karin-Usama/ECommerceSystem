using System.ComponentModel.DataAnnotations;

namespace ECommerceSystem.BLL
{
    public class RegisterDto
    {
        [Required][MinLength(2)] public required string FirstName { get; set; }
        [Required][MinLength(2)] public required string LastName { get; set; }
        [Required][EmailAddress] public required string Email { get; set; }
        [Required][MinLength(4)] public required string Password { get; set; }
    }
}
