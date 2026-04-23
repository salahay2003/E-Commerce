namespace ECommerce.DAL
{
    public interface IUnitOfWork
    {
        public ICartRepository CartRepository { get; }
        public IProductRepository ProductRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IOrderRepository OrderRepository { get; }
        Task SaveAsync();
    }
}
