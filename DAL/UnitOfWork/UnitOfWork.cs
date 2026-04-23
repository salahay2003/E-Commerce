namespace ECommerce.DAL
{

    public class UnitOfWork : IUnitOfWork
    {
        public ICartRepository CartRepository { get; }
        public IProductRepository ProductRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IOrderRepository OrderRepository { get; }
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ICartRepository cartRepository,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IOrderRepository orderRepository,
            ApplicationDbContext context)
        {
            CartRepository = cartRepository;
            ProductRepository = productRepository;
            CategoryRepository = categoryRepository;
            OrderRepository = orderRepository;
            _context = context;            
        }


        public async Task SaveAsync()
        {
           await  _context.SaveChangesAsync();
        }
    }
}
