using Microsoft.AspNetCore.Http;

namespace ECommerceSystem.BLL
{
    public interface IImageService
    {
        Task<string?> UploadImageAsync(IFormFile image);
        void DeleteImage(string? imageUrl);
    }
}
