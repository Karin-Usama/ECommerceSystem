namespace ECommerceSystem.BLL
{
    public class OrderReadDto
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public IEnumerable<OrderItemReadDto> Items { get; set; } = new List<OrderItemReadDto>();
    }
}
