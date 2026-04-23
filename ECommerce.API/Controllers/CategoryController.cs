using ECommerce.BLL;
using ECommerce.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryManager _categoryManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageManager _imageManager;
        public CategoryController(ICategoryManager categoryManager, IWebHostEnvironment webHostEnvironment, IImageManager imageManager)
        {
            _categoryManager = categoryManager;
            _webHostEnvironment = webHostEnvironment;
            _imageManager = imageManager;
        }
        [HttpGet]
        public async Task<ActionResult<GeneralResult<IEnumerable<CategoryReadDto>>>> GetAllAsync()
        {
            var result = await _categoryManager.GetAllCategories();
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GeneralResult<CategoryReadDto>>> GetCategoryById([FromRoute]int id)
        {
            var result = await _categoryManager.GetCategoryById(id);
            if(result == null)
            {
                return NotFound(result);
            }
            return Ok(result);  
        }
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<GeneralResult<CategoryReadDto>>> CreateCategory(CategoryCreateDto categoryCreateDto)
        {
            var result = await _categoryManager.Create(categoryCreateDto);
            if(!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost]
        [Route("{id:int}/image")]
        public async Task<ActionResult<GeneralResult<ImageUploadResultDto>>> UploadCategoryImage(
            [FromQuery] ImageUploadDto imageUploadDto,
            [FromRoute] int id)
        {
            var schema = Request.Scheme;
            var host = Request.Host.Value;
            var basePath = _webHostEnvironment.ContentRootPath;
            var result = await _imageManager.UploadAsync(imageUploadDto, basePath, schema, host!);
            if (!result.Success)
            {
                return BadRequest();
            }
            var updateResult = await _categoryManager.UpdateImageAsync(id, result.Data!.imgURL);
            if(!updateResult.Success)
            {
                return NotFound(updateResult);
            }
            return Ok(updateResult);
        }
        [HttpPut]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<GeneralResult<CategoryReadDto>>> EditCategory(CategoryEditDto categoryEditDto)
        {
            var result = await _categoryManager.Edit(categoryEditDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete]
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<GeneralResult<CategoryReadDto>>> DeleteCategory(int id)
        {
            var result = await _categoryManager.Delete(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }



    }
}
