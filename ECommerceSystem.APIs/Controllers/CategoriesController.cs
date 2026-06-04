using ECommerceSystem.BLL;
using ECommerceSystem.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSystem.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryManager _categoryManager;
        public CategoriesController(ICategoryManager categoryManager) => _categoryManager = categoryManager;

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<GeneralResult<IEnumerable<CategoryReadDto>>>> GetAll()
            => Ok(await _categoryManager.GetAllAsync());

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<GeneralResult<CategoryReadDto>>> GetById(int id)
        {
            var result = await _categoryManager.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<GeneralResult<CategoryReadDto>>> Create([FromForm] CategoryCreateDto dto)
        {
            var result = await _categoryManager.CreateAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<GeneralResult<CategoryReadDto>>> Update(int id, [FromForm] CategoryEditDto dto)
        {
            var result = await _categoryManager.UpdateAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<GeneralResult>> Delete(int id)
        {
            var result = await _categoryManager.DeleteAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost("{id:int}/image")]
        [Authorize(Policy = "AdminOnly")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<GeneralResult<CategoryReadDto>>> UploadImage(int id, IFormFile image)
        {
            var result = await _categoryManager.UploadImageAsync(id, image);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
