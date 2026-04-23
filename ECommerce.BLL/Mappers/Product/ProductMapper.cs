namespace ECommerce.BLL
{
    internal class ProductMapper : IProductMapper
    {
        public DAL.Product ProductCreateToDomainConverter(ProductCreateDto productCreateDto)
        {
            return new DAL.Product
            {
                Name = productCreateDto.Name,
                CategoryId = productCreateDto.CategoryId,
                Description = productCreateDto.Description,
                Price = productCreateDto.Price,
                StockQty = productCreateDto.StockQty,
                ImageUrl = productCreateDto.ImageUrl
            };
        }


        public ProductReadDto ProductDomainToReadConverter(DAL.Product product)
        {
            return new ProductReadDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Category = product.Category.Name,
                Price = product.Price,
                StockQty = product.StockQty,
                ImageUrl = product.ImageUrl
            };
        }

        public ProductCreateDto ProductEditToCreateConverter(ProductEditDto productEditDto)
        {
            return new ProductCreateDto
            {
                Name = productEditDto.Name,
                Price = productEditDto.Price,
                CategoryId = productEditDto.CategoryId,
                Description = productEditDto.Description,
                ImageUrl = productEditDto.ImageUrl,
                StockQty = productEditDto.StockQty
            };
        }
    }
}
