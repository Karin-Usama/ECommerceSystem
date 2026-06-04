namespace ECommerceSystem.BLL
{
    public class CartReadDto
    {
        public int Id { get; set; }
        public IEnumerable<CartItemReadDto> Items { get; set; } = new List<CartItemReadDto>();
        public decimal TotalPrice => Items.Sum(i => i.TotalPrice);
    }
}
