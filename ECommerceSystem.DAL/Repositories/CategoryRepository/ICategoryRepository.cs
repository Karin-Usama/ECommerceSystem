namespace ECommerceSystem.DAL
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetByIdWithProductsAsync(int id);
    }
}
