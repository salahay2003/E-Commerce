using ECommerce.DAL;

namespace ECommerce.BLL
{
    public class CategoryMapper : ICategoryMapper
    {
        public Category CategoryCreateToDomainConverter(CategoryCreateDto productCreateDto)
        {
            return new Category
            {
                Name = productCreateDto.Name,
                ImageUrl = productCreateDto.ImageUrl,
            };
        }

        public CategoryReadDto CategoryDomainToReadConverter(Category category)
        {
            return new CategoryReadDto {Id = category.Id, Name = category.Name, ImageUrl = category.ImageUrl };
        }

        public CategoryCreateDto CategoryEditToCreateConverter(CategoryEditDto categoryEditDto)
        {
            return new CategoryCreateDto { Name = categoryEditDto.Name, ImageUrl= categoryEditDto.ImageUrl };   
        }
    }
}
