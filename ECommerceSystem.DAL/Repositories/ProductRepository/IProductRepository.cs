namespace ECommerceSystem.DAL
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<(IEnumerable<Product> Products, int TotalCount)> GetFilteredAsync(
            int? categoryId, string? name, int pageNumber, int pageSize);
        Task<Product?> GetByIdWithCategoryAsync(int id);
    }
}
