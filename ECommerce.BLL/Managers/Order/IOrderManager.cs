using ECommerce.Common;

namespace ECommerce.BLL
{
    public interface IOrderManager
    {
        public Task<GeneralResult<IEnumerable<OrderReadDto>>> GetOrdersByUserId(string userId);
        public Task<GeneralResult<OrderReadDto>>? GetOrderDetails(string userId, int orderId);
        public Task<GeneralResult<OrderReadDto>> PlaceOrder(string userId);
    }
}
