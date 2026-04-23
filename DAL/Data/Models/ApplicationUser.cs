using Microsoft.AspNetCore.Identity;

namespace ECommerce.DAL
{
    public class ApplicationUser : IdentityUser
    {
        // Properties
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        //Navigation Properties
        public virtual Cart Cart { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } =
            new HashSet<Order>();
        //----------------------------------------
    }
}
