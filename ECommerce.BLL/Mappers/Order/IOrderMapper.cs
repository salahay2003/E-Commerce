using ECommerce.DAL;

namespace ECommerce.BLL
{
    public interface IOrderMapper
    {
        public OrderReadDto OrderDomainToReadConverter(Order order);
    }
}
