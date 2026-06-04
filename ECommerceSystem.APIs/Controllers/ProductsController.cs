using ECommerceSystem.BLL;
using ECommerceSystem.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSystem.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductManager _productManager;
        public ProductsController(IProductManager productManager) => _productManager = productManager;

        [HttpGet]
        public async Task<ActionResult<GeneralResult<ProductPaginatedDto>>> GetAll([FromQuery] ProductQueryDto query)
            => Ok(await _productManager.GetProductsAsync(query));

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GeneralResult<ProductReadDto>>> GetById(int id)
        {
            var result = await _productManager.GetProductByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<GeneralResult<ProductReadDto>>> Create([FromForm] ProductCreateDto dto)
        {
            var result = await _productManager.CreateProductAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<GeneralResult<ProductReadDto>>> Update(int id, [FromForm] ProductEditDto dto)
        {
            var result = await _productManager.UpdateProductAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<GeneralResult>> Delete(int id)
        {
            var result = await _productManager.DeleteProductAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost("{id:int}/image")]
        [Authorize(Policy = "AdminOnly")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<GeneralResult<ProductReadDto>>> UploadImage(int id, IFormFile image)
        {
            var result = await _productManager.UploadProductImageAsync(id, image);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
