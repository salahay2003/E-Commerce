using ECommerce.DAL;
using FluentValidation;

namespace ECommerce.BLL
{
    public class CartValidator : AbstractValidator<CartGetDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;


            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .WithErrorCode("ERR-05")
                .InclusiveBetween(1, 100)
                .WithMessage("Quantity must be between 1 and 100")
                .WithErrorCode("ERR-05");

            RuleFor(c => c.ProductId)
                .GreaterThan(0)
                .WithMessage("Product Id nust be greater then 0")
                .WithErrorCode("ERR-07")
                .MustAsync(CheckProductExit)
                .WithMessage("Product does not exit")
                .WithErrorCode("ERR-07");

        }

        private async Task<bool> CheckProductExit(int productId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.ProductRepository.GetByIdAsync(productId) != null;
        }

    }
}
