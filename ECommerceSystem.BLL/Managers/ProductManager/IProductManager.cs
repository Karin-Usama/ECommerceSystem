using ECommerceSystem.Common;

namespace ECommerceSystem.BLL
{
    public interface IProductManager
    {
        Task<GeneralResult<ProductPaginatedDto>> GetProductsAsync(ProductQueryDto query);
        Task<GeneralResult<ProductReadDto>> GetProductByIdAsync(int id);
        Task<GeneralResult<ProductReadDto>> CreateProductAsync(ProductCreateDto dto);
        Task<GeneralResult<ProductReadDto>> UpdateProductAsync(int id, ProductEditDto dto);
        Task<GeneralResult> DeleteProductAsync(int id);
        Task<GeneralResult<ProductReadDto>> UploadProductImageAsync(int id, Microsoft.AspNetCore.Http.IFormFile image);
    }
}
