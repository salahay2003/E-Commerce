namespace ECommerce.DAL
{
    public class Cart
    {
        // Properties
        public int Id { get; set; }
        // Navigation properties
        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<CartItem> CartItems { get; set; } =
            new HashSet<CartItem>();
        //-------------------------------------
    }
}
