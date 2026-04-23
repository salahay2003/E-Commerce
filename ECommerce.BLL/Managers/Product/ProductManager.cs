using ECommerce.Common;
using ECommerce.DAL;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ECommerce.BLL
{
    public class ProductManager : IProductManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IErrorMapper _errorMapper;
        private readonly IValidator<ProductCreateDto> _productCreateDtoValidator;
        private readonly IProductMapper _productMapper;

        public ProductManager(IUnitOfWork unitOfWork,
            IValidator<ProductCreateDto> productCreateValidator,
            IErrorMapper errorMapper,
            IProductMapper productMapper)
        {
            _unitOfWork = unitOfWork;
            _errorMapper = errorMapper;
            _productCreateDtoValidator = productCreateValidator;
            _productMapper = productMapper;
        }
        public async Task<GeneralResult<PagedResult<ProductReadDto>>> GetProductsPaginationAsync(PaginationParameters paginationParameters, ProductFilterParameters productFilterParameters)
        {
           var pagedResult = await _unitOfWork
                .ProductRepository
                .GetProductsPaginationAsync(paginationParameters, productFilterParameters);
            if (pagedResult.Items == null)
            {
                 return GeneralResult<PagedResult<ProductReadDto>>.NotFound();
            }
            var productReadDtos = pagedResult.Items.Select(p =>
                                                            _productMapper.ProductDomainToReadConverter(p))
                                                            .ToList();
            var pagedresultDto = new PagedResult<ProductReadDto>
            {
                Items = productReadDtos,
                Metadata  = pagedResult.Metadata,
            };
            return GeneralResult<PagedResult<ProductReadDto>>.SuccessResult(pagedresultDto);
        }

        public async Task<GeneralResult<IEnumerable<ProductReadDto>>> GetProductsAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllWithCategoryAsync();
            var productReadDto = products.Select(p =>                                                   _productMapper.ProductDomainToReadConverter(p));
            return GeneralResult<IEnumerable<ProductReadDto>>.SuccessResult(productReadDto);
        }

        public async Task<GeneralResult<ProductReadDto>> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdWithCategoryAsync(id);
            if (product == null) 
            {
               return GeneralResult<ProductReadDto>.NotFound();
            }
            var productReadDto = _productMapper.ProductDomainToReadConverter(product);
            return GeneralResult<ProductReadDto>.SuccessResult(productReadDto);
        }

        public async Task<GeneralResult<ProductReadDto>> CreateAsync(ProductCreateDto productCreateDto)
        {
            var validationResult = await _productCreateDtoValidator.ValidateAsync(productCreateDto);
            if (!validationResult.IsValid)
            {
                var error = _errorMapper.MapError(validationResult);
                return GeneralResult<ProductReadDto>.FailResult(error);
            }
           
            var product = _productMapper.ProductCreateToDomainConverter(productCreateDto);
             _unitOfWork.ProductRepository.Add(product);
            await _unitOfWork.SaveAsync();
            var productReadDto = _productMapper.ProductDomainToReadConverter(product);
            return GeneralResult<ProductReadDto>.SuccessResult(productReadDto);
        }

        public async Task<GeneralResult<ProductReadDto>> EditAsync(ProductEditDto productEditDto)
        {
            var productValidator = _productMapper.ProductEditToCreateConverter(productEditDto);
            var validationResult = await _productCreateDtoValidator.ValidateAsync(productValidator);
            if(!validationResult.IsValid)
            {
                var error = _errorMapper.MapError(validationResult);
                return GeneralResult<ProductReadDto>.FailResult(error);
            }
            var productToEdit = await _unitOfWork.ProductRepository.GetByIdWithCategoryAsync(productEditDto.Id);
            if (productToEdit == null) 
            {
                return GeneralResult<ProductReadDto>.NotFound();
            }
            productToEdit.Name = productEditDto.Name;
            productToEdit.Description = productEditDto.Description;
            productToEdit.CategoryId = productEditDto.CategoryId;
            productToEdit.StockQty = productEditDto.StockQty;
            productToEdit.Price = productEditDto.Price;

            await _unitOfWork.SaveAsync();
            var productReadDto = _productMapper.ProductDomainToReadConverter(productToEdit);
            return GeneralResult<ProductReadDto>.SuccessResult(productReadDto);
        }
        public async Task<GeneralResult<ProductReadDto>> DeleteAsync(int id)
        {
            var productInDB = await _unitOfWork.ProductRepository.GetByIdWithCategoryAsync(id);
            if (productInDB == null) 
                {
                return GeneralResult<ProductReadDto>.NotFound();
            }
            var productReadDto = _productMapper.ProductDomainToReadConverter(productInDB);
            _unitOfWork.ProductRepository.Delete(productInDB);
            await _unitOfWork.SaveAsync();
            return GeneralResult<ProductReadDto>.SuccessResult(productReadDto);
        }

        public async Task<GeneralResult<ProductReadDto>> UpdateImageAsync(int id, string url)
        {
            var productInDb = await _unitOfWork.ProductRepository.GetByIdWithCategoryAsync(id);
            if (productInDb == null)
            {
                return GeneralResult<ProductReadDto>.NotFound();
            }
            productInDb.ImageUrl = url;
            await _unitOfWork.SaveAsync();
            var productReadDto = _productMapper.ProductDomainToReadConverter(productInDb);
            return GeneralResult<ProductReadDto>.SuccessResult(productReadDto);
        }
    }
}
