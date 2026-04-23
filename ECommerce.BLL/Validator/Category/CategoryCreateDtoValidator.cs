using ECommerce.DAL;
using FluentValidation;

namespace ECommerce.BLL
{
    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryCreateDtoValidator(IUnitOfWork unitOfWork)
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
                .WithMessage("Category already exists")
                .WithErrorCode("ERR-04");
        }
        private async Task<bool> CheckNameIsUnique(string name, CancellationToken cancellationToken)
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            var isUnique = categories.Any(c => c.Name == name);
            return !isUnique;
        }
    }
}
