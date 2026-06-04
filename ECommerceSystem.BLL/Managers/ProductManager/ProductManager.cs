using ECommerceSystem.Common;
using ECommerceSystem.DAL;
using Microsoft.AspNetCore.Http;

namespace ECommerceSystem.BLL
{
    public class ProductManager : IProductManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public ProductManager(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task<GeneralResult<ProductPaginatedDto>> GetProductsAsync(ProductQueryDto query)
        {
            var (products, totalCount) = await _unitOfWork.ProductRepository
                .GetFilteredAsync(query.CategoryId, query.Name, query.PageNumber, query.PageSize);

            var dtos = products.Select(p => MapToReadDto(p)).ToList();

            return GeneralResult<ProductPaginatedDto>.SuccessResult(new ProductPaginatedDto
            {
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                Products = dtos
            });
        }

        public async Task<GeneralResult<ProductReadDto>> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdWithCategoryAsync(id);
            if (product == null) return GeneralResult<ProductReadDto>.NotFound();
            return GeneralResult<ProductReadDto>.SuccessResult(MapToReadDto(product));
        }

        public async Task<GeneralResult<ProductReadDto>> CreateProductAsync(ProductCreateDto dto)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(dto.CategoryId);
            if (category == null) return GeneralResult<ProductReadDto>.NotFound("Category not found.");

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId,
                ImageUrl = dto.Image != null ? await _imageService.UploadImageAsync(dto.Image) : null
            };

            _unitOfWork.ProductRepository.Insert(product);
            await _unitOfWork.SaveChangesAsync();
            product.Category = category;

            return GeneralResult<ProductReadDto>.SuccessResult(MapToReadDto(product));
        }

        public async Task<GeneralResult<ProductReadDto>> UpdateProductAsync(int id, ProductEditDto dto)
        {
            if (id != dto.Id) return GeneralResult<ProductReadDto>.Fail("ID mismatch.");

            var product = await _unitOfWork.ProductRepository.GetByIdWithCategoryAsync(id);
            if (product == null) return GeneralResult<ProductReadDto>.NotFound();

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(dto.CategoryId);
            if (category == null) return GeneralResult<ProductReadDto>.NotFound("Category not found.");

            if (dto.Image != null)
            {
                _imageService.DeleteImage(product.ImageUrl);
                product.ImageUrl = await _imageService.UploadImageAsync(dto.Image);
            }

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.CategoryId = dto.CategoryId;
            product.Category = category;

            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return GeneralResult<ProductReadDto>.SuccessResult(MapToReadDto(product));
        }

        public async Task<GeneralResult> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null) return GeneralResult.NotFound();

            _imageService.DeleteImage(product.ImageUrl);
            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync();
            return GeneralResult.SuccessResult();
        }

        public async Task<GeneralResult<ProductReadDto>> UploadProductImageAsync(int id, IFormFile image)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdWithCategoryAsync(id);
            if (product == null) return GeneralResult<ProductReadDto>.NotFound();

            _imageService.DeleteImage(product.ImageUrl);
            product.ImageUrl = await _imageService.UploadImageAsync(image);

            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return GeneralResult<ProductReadDto>.SuccessResult(MapToReadDto(product));
        }

        private static ProductReadDto MapToReadDto(Product p) => new()
        {
            Id = p.Id, Name = p.Name, Description = p.Description,
            Price = p.Price, Stock = p.Stock, ImageUrl = p.ImageUrl,
            CategoryId = p.CategoryId, CategoryName = p.Category?.Name
        };
    }
}
