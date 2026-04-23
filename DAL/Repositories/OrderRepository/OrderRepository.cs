using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context) { }
       
        public async Task<Order?> PlaceOrderAync(string userID)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userID);
            if(cart == null || !cart.CartItems.Any())
                return null;
            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                UserId = userID,
                Status = OrderStatus.Pending,
                TotalAmount = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity),
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Product.Price
                }).ToList()
            };
            await _context.Orders.AddAsync(order);
            //-Clear Cart
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();
            return order;
        }   
        public async Task<Order?> GetOrderByIdAsync(int orderId, string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        }

        public async Task<IEnumerable<Order>?> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

    }
}
