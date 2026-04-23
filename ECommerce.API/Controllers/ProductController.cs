using ECommerce.BLL;
using ECommerce.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductManager _productManager;
        private readonly IImageManager _imageManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductManager productManager, IImageManager imageManager, IWebHostEnvironment webHostEnvironment)
        {
            _productManager = productManager;
            _imageManager = imageManager;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<GeneralResult<IEnumerable<ProductReadDto>>>> GetAllAsync()
        {
            var result = await _productManager.GetProductsAsync();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetAllPagination")]
        public async Task<ActionResult<GeneralResult<IEnumerable<ProductReadDto>>>> GetAllPaginationAsync(
            [FromQuery]PaginationParameters paginationParameters, [FromQuery] ProductFilterParameters productFilterParameters)
        {
            var result = await _productManager.GetProductsPaginationAsync(paginationParameters, productFilterParameters);
            return Ok(result);
        }
        [HttpPost]
        [Route("{id:int}/image")]
        public async Task<ActionResult<GeneralResult<ProductReadDto>>> UploadProductImage(
            [FromForm]ImageUploadDto imageUploadDto,
            [FromRoute] int id)
        {
            var schema = Request.Scheme;
            var host = Request.Host.Value;
            var basePath = _webHostEnvironment.ContentRootPath;
            var result = await _imageManager.UploadAsync(imageUploadDto, basePath, schema, host!);
            if(!result.Success)
            {
                return BadRequest();
            }
            var updateResult = await _productManager.UpdateImageAsync(id, result.Data!.imgURL);
            if (!updateResult.Success)
                return NotFound(updateResult);
            return Ok(updateResult);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GeneralResult<ProductReadDto>>> GetById([FromRoute]int id)
        {
            var result = await _productManager.GetProductByIdAsync(id);
            if(!result.Success)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        [HttpPost]
        [Route("CreateProduct")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<GeneralResult<ProductReadDto>>> CreateProduct(ProductCreateDto productCreateDto)
        {
            var result = await _productManager.CreateAsync(productCreateDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPut]
        [Route("EditProduct")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<GeneralResult<ProductReadDto>>> EditProduct(ProductEditDto productEditDto)
        {
            var result = await _productManager.EditAsync(productEditDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete]
        [Route("DeleteProduct")]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<GeneralResult<ProductReadDto>>> DeleteProduct(int id)
        {
            var result = await _productManager.DeleteAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


    }
}
