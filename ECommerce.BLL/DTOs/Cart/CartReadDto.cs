using ECommerce.DAL;

namespace ECommerce.BLL
{
    public class CartReadDto
    {
        public int Id { get; set; }
        public IEnumerable<CartItemReadDto>? CartItems { get; set; }
    }
}
