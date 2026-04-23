using ECommerce.DAL;

namespace ECommerce.BLL
{
    public interface ICategoryMapper 
    {
        CategoryCreateDto CategoryEditToCreateConverter(CategoryEditDto productCreateDto);
        Category CategoryCreateToDomainConverter(CategoryCreateDto productCreateDto);
        CategoryReadDto CategoryDomainToReadConverter(Category productCreateDto);
    }
}
