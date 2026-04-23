namespace ECommerce.DAL
{
    public class CartItem
    {
        //Properties
        public int Id { get; set; }
        public int Quantity { get; set; }
        //Navigation Properties
        public int ProductId { get; set; }
        public int CartId { get; set; }
        public virtual Product Product { get; set; } = null!;
        public virtual Cart Cart { get; set; } = null!;
        //------------------------------------

    }
}
