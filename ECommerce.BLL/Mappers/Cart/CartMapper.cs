using ECommerce.DAL;

namespace ECommerce.BLL
{
    public class CartMapper : ICartMapper
    {
        public CartReadDto CartDomainToReadConverter(Cart cart)
        {
            return new CartReadDto
            {
                Id = cart.Id,
                CartItems = CartItemDomainToRead(cart.CartItems)
            };
        }
        public IEnumerable<CartItemReadDto> CartItemDomainToRead(IEnumerable<CartItem> cartItems)
        {
            var result = cartItems.Select(ci => new CartItemReadDto
            {
                ProductId = ci.ProductId,
                ProductName = ci.Product.Name,
                Price = ci.Product.Price,
                Quantity = ci.Quantity
            });
            return result;
        }
    }
}
