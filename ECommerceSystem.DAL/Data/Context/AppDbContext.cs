using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerceSystem.DAL
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public AppDbContext() : base() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AuditLog();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AuditLog()
        {
            var now = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries<IAuditEntity>())
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedAt = now;
                else if (entry.State == EntityState.Modified)
                    entry.Entity.UpdatedAt = now;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            var createdAt = new DateTime(2026, 1, 1, 0, 0, 0);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics",  Description = "Electronic devices",  CreatedAt = createdAt },
                new Category { Id = 2, Name = "Accessories",  Description = "Phone accessories",   CreatedAt = createdAt },
                new Category { Id = 3, Name = "Computers",    Description = "Laptops and desktops", CreatedAt = createdAt }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "iPhone 15",    Description = "Apple smartphone",   Price = 25000, Stock = 20, CategoryId = 1, CreatedAt = createdAt },
                new Product { Id = 2, Name = "Samsung S24",  Description = "Samsung smartphone", Price = 22000, Stock = 15, CategoryId = 1, CreatedAt = createdAt },
                new Product { Id = 3, Name = "USB-C Cable",  Description = "Fast charging cable",Price = 150,   Stock = 100, CategoryId = 2, CreatedAt = createdAt },
                new Product { Id = 4, Name = "MacBook Pro",  Description = "Apple laptop",       Price = 60000, Stock = 10, CategoryId = 3, CreatedAt = createdAt },
                new Product { Id = 5, Name = "Dell XPS 15",  Description = "Dell laptop",        Price = 45000, Stock = 8,  CategoryId = 3, CreatedAt = createdAt }
            );
        }

        public virtual DbSet<Category> Categories => Set<Category>();
        public virtual DbSet<Product> Products => Set<Product>();
        public virtual DbSet<Cart> Carts => Set<Cart>();
        public virtual DbSet<CartItem> CartItems => Set<CartItem>();
        public virtual DbSet<Order> Orders => Set<Order>();
        public virtual DbSet<OrderItem> OrderItems => Set<OrderItem>();
    }
}
