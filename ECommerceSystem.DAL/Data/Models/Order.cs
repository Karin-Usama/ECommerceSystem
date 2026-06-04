namespace ECommerceSystem.DAL
{
    public enum OrderStatus { Pending, Processing, Shipped, Delivered, Cancelled }

    public class Order : IAuditEntity
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
