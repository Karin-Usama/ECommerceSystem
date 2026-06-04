using ECommerceSystem.BLL;
using ECommerceSystem.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceSystem.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartManager _cartManager;
        public CartController(ICartManager cartManager) => _cartManager = cartManager;

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<ActionResult<GeneralResult<CartReadDto>>> GetCart()
            => Ok(await _cartManager.GetCartAsync(GetUserId()));

        [HttpPost]
        public async Task<ActionResult<GeneralResult<CartReadDto>>> AddToCart([FromBody] AddToCartDto dto)
        {
            var result = await _cartManager.AddToCartAsync(GetUserId(), dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<ActionResult<GeneralResult<CartReadDto>>> UpdateCartItem([FromBody] UpdateCartItemDto dto)
        {
            var result = await _cartManager.UpdateCartItemAsync(GetUserId(), dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{productId:int}")]
        public async Task<ActionResult<GeneralResult>> RemoveFromCart(int productId)
        {
            var result = await _cartManager.RemoveFromCartAsync(GetUserId(), productId);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
