using ECommerce.DAL;

namespace ECommerce.BLL
{
    public class OrderMapper : IOrderMapper
    {
        public OrderReadDto OrderDomainToReadConverter(Order order)
        {
            return new OrderReadDto
            {
                Id = order.Id,
                OrderItems = OrderItemDomainToRead(order.OrderItems),
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount
                
            };
        }
        public IEnumerable<OrderItemReadDto> OrderItemDomainToRead(IEnumerable<OrderItem> orderItems)
        {
            var result = orderItems.Select(ci => new OrderItemReadDto
            {
                ProductId = ci.ProductId,
                ProductName = ci.Product.Name,
                Price = ci.Product.Price,
                Quantity = ci.Quantity
            });
            return result;
        }
    }
}
