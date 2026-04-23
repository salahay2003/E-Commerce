using ECommerce.Common;
using ECommerce.DAL;
using FluentValidation;

namespace ECommerce.BLL
{
    public class CartManager : ICartManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartMapper _cartMapper;
        private readonly IValidator<CartGetDto> _validator;
        private readonly IErrorMapper _errorMapper;

        public CartManager(IUnitOfWork unitOfWork, ICartMapper cartMapper, IErrorMapper errorMapper, IValidator<CartGetDto> validator)
        {
            _unitOfWork = unitOfWork;
            _cartMapper = cartMapper;
            _errorMapper = errorMapper;
            _validator = validator;
        }
        public async Task<GeneralResult<CartReadDto>> GetUserCart(string userId)
        {
            var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) {
                return GeneralResult<CartReadDto>.SuccessResult(new CartReadDto());
            }
            var cartReadDto = _cartMapper.CartDomainToReadConverter(cart);
            return GeneralResult<CartReadDto>.SuccessResult(cartReadDto);
        }

        public async Task<GeneralResult<CartReadDto>> AddToCart(string userId, CartGetDto cartGetDto)
        {
            var validationResult = await _validator.ValidateAsync(cartGetDto);
            if(!validationResult.IsValid)
            {
                var errors = _errorMapper.MapError(validationResult);
                return GeneralResult<CartReadDto>.FailResult(errors);
            }
            await _unitOfWork.CartRepository.AddItemToCartAsync(userId, cartGetDto.ProductId, cartGetDto.Quantity?? 1);
            await _unitOfWork.SaveAsync();
            var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
            var cartReadDto = _cartMapper.CartDomainToReadConverter(cart!);
            return GeneralResult<CartReadDto>.SuccessResult(cartReadDto);
        }

        public async Task<GeneralResult<CartReadDto>> RemoveFromCart(string userId, int productId)
        {
            await _unitOfWork.CartRepository.RemoveItemFromCartAsync(userId,productId);
            await _unitOfWork.SaveAsync();
            var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return GeneralResult<CartReadDto>.SuccessResult(new CartReadDto());
            }
            var cartReadDto = _cartMapper.CartDomainToReadConverter(cart);
            return GeneralResult<CartReadDto>.SuccessResult(cartReadDto);
        }

        public async Task<GeneralResult<CartReadDto>> UpdateCartItem(string userId, CartGetDto cartGetDto)
        {
            if (cartGetDto.Quantity <= 0)
                return GeneralResult<CartReadDto>.FailResult("Quantity can not be less than 1"); 

            await _unitOfWork.CartRepository.UpdateItemQuantityAsync(userId, cartGetDto.ProductId, cartGetDto.Quantity ?? 1);
            await _unitOfWork.SaveAsync();

            var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return GeneralResult<CartReadDto>.SuccessResult(new CartReadDto());
            }

            var cartReadDto = _cartMapper.CartDomainToReadConverter(cart);
            return GeneralResult<CartReadDto>.SuccessResult(cartReadDto);
        }

    
    }
}
