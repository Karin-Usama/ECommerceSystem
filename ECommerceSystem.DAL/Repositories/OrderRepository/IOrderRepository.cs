namespace ECommerceSystem.DAL
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order?> GetOrderByIdWithDetailsAsync(int orderId, string userId);
    }
}
