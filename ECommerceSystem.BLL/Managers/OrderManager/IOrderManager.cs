using ECommerceSystem.Common;

namespace ECommerceSystem.BLL
{
    public interface IOrderManager
    {
        Task<GeneralResult<OrderReadDto>> PlaceOrderAsync(string userId);
        Task<GeneralResult<IEnumerable<OrderReadDto>>> GetOrdersAsync(string userId);
        Task<GeneralResult<OrderReadDto>> GetOrderByIdAsync(string userId, int orderId);
    }
}
