namespace ECommerce.DAL
{
    public class Category : IAuditableEntity
    {
        //Properties
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        // Navigation properties
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
        //--------------------------------------------------------------
    }
}
