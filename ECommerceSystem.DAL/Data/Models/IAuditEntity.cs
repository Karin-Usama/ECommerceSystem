namespace ECommerceSystem.DAL
{
    public interface IAuditEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
