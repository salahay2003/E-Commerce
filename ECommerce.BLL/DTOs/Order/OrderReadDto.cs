namespace ECommerce.BLL
{
    public class OrderReadDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public IEnumerable<OrderItemReadDto>? OrderItems { get; set; }
    }
}
