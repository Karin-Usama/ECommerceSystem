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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderManager _orderManager;
        public OrdersController(IOrderManager orderManager) => _orderManager = orderManager;

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpPost]
        public async Task<ActionResult<GeneralResult<OrderReadDto>>> PlaceOrder()
        {
            var result = await _orderManager.PlaceOrderAsync(GetUserId());
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        public async Task<ActionResult<GeneralResult<IEnumerable<OrderReadDto>>>> GetOrders()
            => Ok(await _orderManager.GetOrdersAsync(GetUserId()));

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GeneralResult<OrderReadDto>>> GetOrderById(int id)
        {
            var result = await _orderManager.GetOrderByIdAsync(GetUserId(), id);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
