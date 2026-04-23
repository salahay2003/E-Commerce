using ECommerce.Common;

namespace ECommerce.BLL
{
    public interface ICartManager
    {
        public Task<GeneralResult<CartReadDto>> GetUserCart(string userId);
        public Task<GeneralResult<CartReadDto>> AddToCart(string userId, CartGetDto cartGetDto);
        public Task<GeneralResult<CartReadDto>> RemoveFromCart(string userId, int productId);
        public Task<GeneralResult<CartReadDto>> UpdateCartItem(string userId, CartGetDto cartGetDto);
    }
}
