using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context) { }
       
        public async Task<Cart?> GetCartByUserIdAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            return cart;
        }
        public async Task AddItemToCartAsync(string userId, int productId, int quantity)
        {
            var cart = await GetCartByUserIdAsync(userId);
            if(cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
                await _context.Carts.AddAsync(cart);
            }
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if(existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                });
            }
        }


        public async Task RemoveItemFromCartAsync(string userId, int productId)
        {
            var cart = await GetCartByUserIdAsync(userId);
            if(cart == null) 
                return;

            var productToRemove = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if(productToRemove is not null)
            {
                _context.CartItems.Remove(productToRemove);
            }
        }

        public async Task UpdateItemQuantityAsync(string userid, int productId, int quantity)
        {
            var cart = await GetCartByUserIdAsync(userid);
            if(cart == null) 
                return;

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
            }
        }
    }
}
