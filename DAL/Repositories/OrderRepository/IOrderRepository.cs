namespace ECommerce.DAL
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> PlaceOrderAync(string userID);
        Task<IEnumerable<Order>?> GetOrdersByUserIdAsync(string userId);
        Task<Order?> GetOrderByIdAsync(int orderId, string userId);
    }
}
