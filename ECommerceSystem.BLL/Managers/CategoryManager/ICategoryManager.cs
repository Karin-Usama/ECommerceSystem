using ECommerceSystem.Common;
using Microsoft.AspNetCore.Http;

namespace ECommerceSystem.BLL
{
    public interface ICategoryManager
    {
        Task<GeneralResult<IEnumerable<CategoryReadDto>>> GetAllAsync();
        Task<GeneralResult<CategoryReadDto>> GetByIdAsync(int id);
        Task<GeneralResult<CategoryReadDto>> CreateAsync(CategoryCreateDto dto);
        Task<GeneralResult<CategoryReadDto>> UpdateAsync(int id, CategoryEditDto dto);
        Task<GeneralResult> DeleteAsync(int id);
        Task<GeneralResult<CategoryReadDto>> UploadImageAsync(int id, IFormFile image);
    }
}
