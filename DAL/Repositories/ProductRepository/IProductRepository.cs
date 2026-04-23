using ECommerce.Common;

namespace ECommerce.DAL
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        /*------------------------------------------------------------------*/
        Task<IEnumerable<Product>> GetAllWithCategoryAsync();
        /*------------------------------------------------------------------*/
        Task<Product?> GetByIdWithCategoryAsync(int ProductID);
        /*------------------------------------------------------------------*/
        Task<PagedResult<Product>> GetProductsPaginationAsync(PaginationParameters? paginationParameters,
            ProductFilterParameters? productFilterParameters);
        /*------------------------------------------------------------------*/
    }
}
