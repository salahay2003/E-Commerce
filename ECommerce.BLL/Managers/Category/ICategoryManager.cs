using ECommerce.Common;

namespace ECommerce.BLL
{
    public interface ICategoryManager
    {
        public Task<GeneralResult<IEnumerable<CategoryReadDto>>> GetAllCategories();
        public Task<GeneralResult<CategoryReadDto>> GetCategoryById(int id);
        public Task<GeneralResult<CategoryReadDto>> Create(CategoryCreateDto categoryCreateDto);
        public Task<GeneralResult<CategoryReadDto>> Edit(CategoryEditDto categoryEditDto);
        public Task<GeneralResult<CategoryReadDto>> Delete(int id);
        public Task<GeneralResult<CategoryReadDto>> UpdateImageAsync(int id, string url);
    }
}
