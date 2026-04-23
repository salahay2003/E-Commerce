namespace ECommerce.DAL
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetCartByUserIdAsync(string userId);
        Task AddItemToCartAsync(string userId, int productId, int quantity);
        Task RemoveItemFromCartAsync(string userId, int productId);
        Task UpdateItemQuantityAsync(string userid, int productId, int quantity);
    }
}
