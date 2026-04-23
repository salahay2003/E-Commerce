using ECommerce.Common;
using ECommerce.DAL;
using FluentValidation;

namespace ECommerce.BLL
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IErrorMapper _errorMapper;
        private readonly IValidator<CategoryCreateDto> _categoryCreateDtoValidator;
        private readonly ICategoryMapper _categoryMapper;
        public CategoryManager(IUnitOfWork unitOfWork,
            IErrorMapper errorMapper,
            IValidator<CategoryCreateDto> validator,
            ICategoryMapper categoryMapper)
        {
            _errorMapper = errorMapper;
            _categoryCreateDtoValidator = validator;
            _unitOfWork = unitOfWork;
            _categoryMapper = categoryMapper;
        }
        public async Task<GeneralResult<IEnumerable<CategoryReadDto>>> GetAllCategories()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            var categoriesReadDto = categories.Select(c => 
                                    _categoryMapper.CategoryDomainToReadConverter(c));
            return GeneralResult<IEnumerable<CategoryReadDto>>.SuccessResult(categoriesReadDto);
        }

        public async Task<GeneralResult<CategoryReadDto>> GetCategoryById(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null) {
                return GeneralResult<CategoryReadDto>.NotFound();
            }
            var categoryReadDto = _categoryMapper.CategoryDomainToReadConverter(category);
            return GeneralResult<CategoryReadDto>.SuccessResult(categoryReadDto);
        }
        public async Task<GeneralResult<CategoryReadDto>> Create(CategoryCreateDto categoryCreateDto)
        {
            var validationResult = await _categoryCreateDtoValidator.ValidateAsync(categoryCreateDto);
            if(!validationResult.IsValid)
            {
                var errors = _errorMapper.MapError(validationResult);
                return GeneralResult<CategoryReadDto>.FailResult(errors);
            }
            var newCategory = _categoryMapper.CategoryCreateToDomainConverter(categoryCreateDto);
            _unitOfWork.CategoryRepository.Add(newCategory);
            await _unitOfWork.SaveAsync();
            var categoryReadDto = _categoryMapper.CategoryDomainToReadConverter(newCategory);
            return GeneralResult<CategoryReadDto>.SuccessResult(categoryReadDto);
        }
        public async Task<GeneralResult<CategoryReadDto>> Edit(CategoryEditDto categoryEditDto)
        {
            var categoryValidator = _categoryMapper.CategoryEditToCreateConverter(categoryEditDto);
            var validationResult = await _categoryCreateDtoValidator.ValidateAsync(categoryValidator);
            if (!validationResult.IsValid)
            {
                var errors = _errorMapper.MapError(validationResult);
                return GeneralResult<CategoryReadDto>.FailResult(errors);
            }
            var categoryToEdit = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryEditDto.Id);
            if(categoryToEdit == null)
            {
                return GeneralResult<CategoryReadDto>.NotFound();
            }
            categoryToEdit.Name = categoryEditDto.Name;
            categoryToEdit.ImageUrl = categoryEditDto.ImageUrl;
            await _unitOfWork.SaveAsync();
            var categoryReadDto = _categoryMapper.CategoryDomainToReadConverter(categoryToEdit);
            return GeneralResult<CategoryReadDto>.SuccessResult( categoryReadDto);
        }

        public async Task<GeneralResult<CategoryReadDto>> Delete(int id)
        {
            var categoryInDb = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if( categoryInDb == null)
            {
                return GeneralResult<CategoryReadDto>.NotFound();
            }
            var categoryReadDto = _categoryMapper.CategoryDomainToReadConverter(categoryInDb);
            _unitOfWork.CategoryRepository.Delete(categoryInDb);
            await _unitOfWork.SaveAsync();
            return GeneralResult<CategoryReadDto>.SuccessResult(categoryReadDto);
        }

        public async Task<GeneralResult<CategoryReadDto>> UpdateImageAsync(int id, string url)
        {
            var categoryInDb = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if(categoryInDb == null)
            {
                return GeneralResult<CategoryReadDto>.NotFound();
            }
            categoryInDb.ImageUrl = url;
            await _unitOfWork.SaveAsync();
            var categoryReadDto = _categoryMapper.CategoryDomainToReadConverter(categoryInDb);
            return GeneralResult<CategoryReadDto>.SuccessResult(categoryReadDto);
        }
    }
}
