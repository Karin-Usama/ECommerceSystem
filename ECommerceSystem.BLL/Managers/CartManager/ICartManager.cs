using ECommerceSystem.Common;

namespace ECommerceSystem.BLL
{
    public interface ICartManager
    {
        Task<GeneralResult<CartReadDto>> GetCartAsync(string userId);
        Task<GeneralResult<CartReadDto>> AddToCartAsync(string userId, AddToCartDto dto);
        Task<GeneralResult<CartReadDto>> UpdateCartItemAsync(string userId, UpdateCartItemDto dto);
        Task<GeneralResult> RemoveFromCartAsync(string userId, int productId);
    }
}
