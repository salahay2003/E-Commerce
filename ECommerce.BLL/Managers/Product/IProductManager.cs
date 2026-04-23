using ECommerce.Common;

namespace ECommerce.BLL
{
    public interface IProductManager
    {
        Task<GeneralResult<IEnumerable<ProductReadDto>>> GetProductsAsync();
        Task<GeneralResult<PagedResult<ProductReadDto>>> GetProductsPaginationAsync(PaginationParameters paginationParameters, ProductFilterParameters productFilterParameters);
        Task<GeneralResult<ProductReadDto>> GetProductByIdAsync(int id);
        Task<GeneralResult<ProductReadDto>> UpdateImageAsync(int id, string url);
        Task<GeneralResult<ProductReadDto>> CreateAsync(ProductCreateDto productCreateVM);
        Task<GeneralResult<ProductReadDto>> EditAsync(ProductEditDto productEditDto);
        Task<GeneralResult<ProductReadDto>> DeleteAsync(int id);
    }
}
