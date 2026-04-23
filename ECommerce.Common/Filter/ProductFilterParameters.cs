namespace ECommerce.Common
{
    public class ProductFilterParameters : BaseFilterParameters 
    {
        public int? CategoryId { get; set; }
        public string? Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
