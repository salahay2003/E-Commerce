using ECommerce.DAL;

namespace ECommerce.BLL
{
    public interface ICartMapper
    {
        public CartReadDto CartDomainToReadConverter(Cart cart);
    }
}
