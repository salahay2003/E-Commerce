using ECommerce.Common;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DAL
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
       
        public ProductRepository(ApplicationDbContext context) : base(context) { }
                    
        public async Task<PagedResult<Product>> GetProductsPaginationAsync(PaginationParameters? paginationParameters, ProductFilterParameters? productFilterParameters)
        {
            IQueryable<Product> query = _context.Products.Include(p=> p.Category).AsNoTracking();
            if(productFilterParameters is not null)
            {
                query = ApplyFilter(query, productFilterParameters);
            }

            var totalCount  = await query.CountAsync();

            var pageNumber = paginationParameters?.PageNumber ?? 1;
            var pageSize = paginationParameters?.PageSize ?? totalCount;

            pageNumber = Math.Max(pageNumber, 1);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var items = await query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<Product>
            {
                Items = items,
                Metadata = new PaginationMetaData
                {
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    TotalPages = totalPages,
                    HasNext = pageNumber < totalPages ,
                    HasPrevious = pageNumber > 1 ,
                   
                },
               
            };

        }
        public async Task<IEnumerable<Product>> GetAllWithCategoryAsync()
        {
            return await _context.Products
                            .Include(p => p.Category)
                            .AsNoTracking()
                            .ToListAsync();
        }

        public async Task<Product?> GetByIdWithCategoryAsync(int ProductID)
        {
            return await _context.Products
                            .Include(p => p.Category)
                           .FirstOrDefaultAsync(p => p.Id == ProductID);
        }

        private IQueryable<Product> ApplyFilter(IQueryable<Product> query, ProductFilterParameters productFilterParameters)
        {

            if (productFilterParameters.MinPrice > 0)
            {
                query = query.Where(p => p.Price > productFilterParameters.MinPrice);
            }

            if (productFilterParameters.MaxPrice > 0)
            {
                query = query.Where(p => p.Price < productFilterParameters.MaxPrice);
            }

            if (productFilterParameters.CategoryId > 0)
            {
                query = query.Where(p => p.CategoryId == productFilterParameters.CategoryId);
            }

            if (productFilterParameters.Name != null)
            {
                query = query.Where(p => p.Name.ToLower() == productFilterParameters.Name.ToLower());
            }

            if (!string.IsNullOrEmpty(productFilterParameters.Search))
            {
                query = query.Where(p => p.Name.Contains(productFilterParameters.Search));
            }

            return query;
        }
    }
}
