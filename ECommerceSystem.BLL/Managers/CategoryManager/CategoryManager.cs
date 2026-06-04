using ECommerceSystem.Common;
using ECommerceSystem.DAL;
using Microsoft.AspNetCore.Http;

namespace ECommerceSystem.BLL
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;

        public CategoryManager(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }

        public async Task<GeneralResult<IEnumerable<CategoryReadDto>>> GetAllAsync()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            return GeneralResult<IEnumerable<CategoryReadDto>>.SuccessResult(
                categories.Select(MapToReadDto).ToList());
        }

        public async Task<GeneralResult<CategoryReadDto>> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null) return GeneralResult<CategoryReadDto>.NotFound();
            return GeneralResult<CategoryReadDto>.SuccessResult(MapToReadDto(category));
        }

        public async Task<GeneralResult<CategoryReadDto>> CreateAsync(CategoryCreateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                ImageUrl = dto.Image != null ? await _imageService.UploadImageAsync(dto.Image) : null
            };
            _unitOfWork.CategoryRepository.Insert(category);
            await _unitOfWork.SaveChangesAsync();
            return GeneralResult<CategoryReadDto>.SuccessResult(MapToReadDto(category));
        }

        public async Task<GeneralResult<CategoryReadDto>> UpdateAsync(int id, CategoryEditDto dto)
        {
            if (id != dto.Id) return GeneralResult<CategoryReadDto>.Fail("ID mismatch.");
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null) return GeneralResult<CategoryReadDto>.NotFound();

            if (dto.Image != null)
            {
                _imageService.DeleteImage(category.ImageUrl);
                category.ImageUrl = await _imageService.UploadImageAsync(dto.Image);
            }

            category.Name = dto.Name;
            category.Description = dto.Description;

            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();
            return GeneralResult<CategoryReadDto>.SuccessResult(MapToReadDto(category));
        }

        public async Task<GeneralResult> DeleteAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null) return GeneralResult.NotFound();
            _imageService.DeleteImage(category.ImageUrl);
            _unitOfWork.CategoryRepository.Delete(category);
            await _unitOfWork.SaveChangesAsync();
            return GeneralResult.SuccessResult();
        }

        public async Task<GeneralResult<CategoryReadDto>> UploadImageAsync(int id, IFormFile image)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null) return GeneralResult<CategoryReadDto>.NotFound();
            _imageService.DeleteImage(category.ImageUrl);
            category.ImageUrl = await _imageService.UploadImageAsync(image);
            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();
            return GeneralResult<CategoryReadDto>.SuccessResult(MapToReadDto(category));
        }

        private static CategoryReadDto MapToReadDto(Category c) => new()
        {
            Id = c.Id, Name = c.Name, Description = c.Description, ImageUrl = c.ImageUrl
        };
    }
}
