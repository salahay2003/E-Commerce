namespace ECommerce.Common
{
    public class PagedResult<T> where T : class
    {
        public IEnumerable<T> Items { get; set; } = [];
        public PaginationMetaData Metadata { get; set; } = new();
    }
}
