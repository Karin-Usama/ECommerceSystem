using ECommerceSystem.Common;
using ECommerceSystem.DAL;

namespace ECommerceSystem.BLL
{
    public class OrderManager : IOrderManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GeneralResult<OrderReadDto>> PlaceOrderAsync(string userId)
        {
            var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.CartItems.Any())
                return GeneralResult<OrderReadDto>.Fail("Cart is empty.");

            foreach (var item in cart.CartItems)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                if (product == null || product.Stock < item.Quantity)
                    return GeneralResult<OrderReadDto>.Fail($"Insufficient stock for product '{item.Product?.Name}'.");
            }

            var order = new Order
            {
                UserId = userId,
                TotalPrice = cart.CartItems.Sum(ci => ci.Product!.Price * ci.Quantity),
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product!.Price
                }).ToList()
            };

            foreach (var item in cart.CartItems)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                product!.Stock -= item.Quantity;
                _unitOfWork.ProductRepository.Update(product);
            }

            _unitOfWork.OrderRepository.Insert(order);
            _unitOfWork.CartRepository.Delete(cart);
            await _unitOfWork.SaveChangesAsync();

            var placed = await _unitOfWork.OrderRepository.GetOrderByIdWithDetailsAsync(order.Id, userId);
            return GeneralResult<OrderReadDto>.SuccessResult(MapToReadDto(placed!));
        }

        public async Task<GeneralResult<IEnumerable<OrderReadDto>>> GetOrdersAsync(string userId)
        {
            var orders = await _unitOfWork.OrderRepository.GetOrdersByUserIdAsync(userId);
            return GeneralResult<IEnumerable<OrderReadDto>>.SuccessResult(orders.Select(MapToReadDto).ToList());
        }

        public async Task<GeneralResult<OrderReadDto>> GetOrderByIdAsync(string userId, int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderByIdWithDetailsAsync(orderId, userId);
            if (order == null) return GeneralResult<OrderReadDto>.NotFound();
            return GeneralResult<OrderReadDto>.SuccessResult(MapToReadDto(order));
        }

        private static OrderReadDto MapToReadDto(Order o) => new()
        {
            Id = o.Id,
            TotalPrice = o.TotalPrice,
            Status = o.Status.ToString(),
            CreatedAt = o.CreatedAt,
            Items = o.OrderItems.Select(oi => new OrderItemReadDto
            {
                ProductId = oi.ProductId,
                ProductName = oi.Product?.Name,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice
            }).ToList()
        };
    }
}
