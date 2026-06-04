namespace ECommerceSystem.DAL
{
    public class Cart : IAuditEntity
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
