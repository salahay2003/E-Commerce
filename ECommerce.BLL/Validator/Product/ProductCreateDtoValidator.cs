using ECommerce.DAL;
using FluentValidation;

namespace ECommerce.BLL
{
    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductCreateDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(p => p.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty.")
                .WithErrorCode("ERR-01")

                .MinimumLength(3)
                .WithMessage("Name at least 3 char")
                .WithErrorCode("ERR-02")

                .MaximumLength(100)
                .WithMessage("Name cannot be longer than 100 char")
                .WithErrorCode("ERR-03")
                .MustAsync(CheckNameIsUnique)
                .WithMessage("Name already exists")
                .WithErrorCode("ERR-04");

            RuleFor(p => p.Price)
                .GreaterThan(0)
                .InclusiveBetween(1, 10000)
                .WithMessage("Price must be between 1 and 10000")
                .WithErrorCode("ERR-05");

            RuleFor(p => p.StockQty)
                .GreaterThan(0)
                .WithMessage("Stock quantity nust be greater then 0")
                .WithErrorCode("ERR-06");

            RuleFor(e => e.CategoryId)
                .GreaterThan(0)
                .WithMessage("Category Id nust be greater then 0")
                .WithErrorCode("ERR-07")
                .MustAsync(CheckCategoryExit)
                .WithMessage("Category does not exit")
                .WithErrorCode("ERR-07");
        }

        private async Task<bool> CheckNameIsUnique(string name, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();
            var isUnique = products.Any(p => p.Name == name);
            return !isUnique;
        }
        private async Task<bool> CheckCategoryExit(int categoryId, CancellationToken cancellationToken)
        {
            return await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId) != null;
        }

    }
}
