using ECommerce.DAL;

namespace ECommerce.BLL
{
    public interface IProductMapper
    {
        //ProductReadDto ProductCreateToReadConverter(ProductCreateDto productCreateDto);
        ProductCreateDto ProductEditToCreateConverter(ProductEditDto productCreateDto);
        Product ProductCreateToDomainConverter(ProductCreateDto productCreateDto);
        ProductReadDto ProductDomainToReadConverter(DAL.Product productCreateDto);
    }
}
