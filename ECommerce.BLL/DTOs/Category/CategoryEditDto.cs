namespace ECommerce.BLL
{
    public class CategoryEditDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? ImageUrl { get; set; }
    }
}
