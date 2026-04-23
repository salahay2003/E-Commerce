using ECommerce.BLL;
using ECommerce.Common;
using ECommerce.DAL;

namespace ECommerce.BLL
{
    public class OrderManager : IOrderManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderMapper _orderMapper;

        public OrderManager(IUnitOfWork unitOfWork, IOrderMapper orderMapper)
        {
            _orderMapper = orderMapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<GeneralResult<OrderReadDto>>? GetOrderDetails(string userId, int orderId)
        {
            var order =await  _unitOfWork.OrderRepository.GetOrderByIdAsync(orderId,userId);
            if(order == null)
            {
                return GeneralResult<OrderReadDto>.NotFound();
            }
            var orderReadDto = _orderMapper.OrderDomainToReadConverter(order);
            return GeneralResult<OrderReadDto>.SuccessResult(orderReadDto);
        }

        public async Task<GeneralResult<IEnumerable<OrderReadDto>>> GetOrdersByUserId(string userId)
        {
            var orders = await _unitOfWork.OrderRepository.GetOrdersByUserIdAsync(userId);
            if (orders == null)
            {
                return GeneralResult<IEnumerable<OrderReadDto>>.NotFound();
            }
            var orderReadDto = orders.Select(o => _orderMapper.OrderDomainToReadConverter(o));
            return GeneralResult<IEnumerable<OrderReadDto>>.SuccessResult(orderReadDto);
        }

        public async Task<GeneralResult<OrderReadDto>> PlaceOrder(string userId)
        {
            var order = await _unitOfWork.OrderRepository.PlaceOrderAync(userId);
            if (order == null)
            {
                return GeneralResult<OrderReadDto>.NotFound("Cart is Empty");
            }
            var orderReadDto = _orderMapper.OrderDomainToReadConverter(order);
            return GeneralResult<OrderReadDto>.SuccessResult(orderReadDto);
        }
    }
}
