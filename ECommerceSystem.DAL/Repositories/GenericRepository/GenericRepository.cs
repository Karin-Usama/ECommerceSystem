using Microsoft.EntityFrameworkCore;

namespace ECommerceSystem.DAL
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _context.Set<T>().AsNoTracking().ToListAsync();

        public async Task<T?> GetByIdAsync(int id)
            => await _context.Set<T>().FindAsync(id);

        public void Insert(T entity) => _context.Add(entity);
        public void Update(T entity) => _context.Update(entity);
        public void Delete(T entity) => _context.Remove(entity);
    }
}
