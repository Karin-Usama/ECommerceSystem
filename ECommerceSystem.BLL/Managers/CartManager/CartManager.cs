using ECommerceSystem.Common;
using ECommerceSystem.DAL;

namespace ECommerceSystem.BLL
{
    public class CartManager : ICartManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResult<CartReadDto>> GetCartAsync(string userId)
        {
            var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
                return GeneralResult<CartReadDto>.SuccessResult(new CartReadDto());
            return GeneralResult<CartReadDto>.SuccessResult(MapToReadDto(cart));
        }

        public async Task<GeneralResult<CartReadDto>> AddToCartAsync(string userId, AddToCartDto dto)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(dto.ProductId);
            if (product == null) return GeneralResult<CartReadDto>.NotFound("Product not found.");
            if (product.Stock < dto.Quantity)
                return GeneralResult<CartReadDto>.Fail($"Only {product.Stock} items in stock.");

            var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _unitOfWork.CartRepository.Insert(cart);
                await _unitOfWork.SaveChangesAsync();
            }

            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);
            if (existingItem != null)
                existingItem.Quantity += dto.Quantity;
            else
                cart.CartItems.Add(new CartItem { CartId = cart.Id, ProductId = dto.ProductId, Quantity = dto.Quantity });

            _unitOfWork.CartRepository.Update(cart);
            await _unitOfWork.SaveChangesAsync();

            cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
            return GeneralResult<CartReadDto>.SuccessResult(MapToReadDto(cart!));
        }

        public async Task<GeneralResult<CartReadDto>> UpdateCartItemAsync(string userId, UpdateCartItemDto dto)
        {
            var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) return GeneralResult<CartReadDto>.NotFound("Cart not found.");

            var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == dto.ProductId);
            if (item == null) return GeneralResult<CartReadDto>.NotFound("Item not found in cart.");

            if (dto.Quantity <= 0)
                cart.CartItems.Remove(item);
            else
                item.Quantity = dto.Quantity;

            _unitOfWork.CartRepository.Update(cart);
            await _unitOfWork.SaveChangesAsync();

            cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
            return GeneralResult<CartReadDto>.SuccessResult(MapToReadDto(cart!));
        }

        public async Task<GeneralResult> RemoveFromCartAsync(string userId, int productId)
        {
            var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) return GeneralResult.NotFound("Cart not found.");

            var item = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (item == null) return GeneralResult.NotFound("Item not found in cart.");

            cart.CartItems.Remove(item);
            _unitOfWork.CartRepository.Update(cart);
            await _unitOfWork.SaveChangesAsync();
            return GeneralResult.SuccessResult();
        }

        private static CartReadDto MapToReadDto(Cart cart) => new()
        {
            Id = cart.Id,
            Items = cart.CartItems.Select(ci => new CartItemReadDto
            {
                ProductId = ci.ProductId,
                ProductName = ci.Product?.Name,
                ImageUrl = ci.Product?.ImageUrl,
                UnitPrice = ci.Product?.Price ?? 0,
                Quantity = ci.Quantity
            }).ToList()
        };
    }
}
