using ECommerce.DAL;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.BLL
{
    public static class BLLServiceExtension 
    {
        public static void AddBLLServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(BLLServiceExtension).Assembly);
            services.AddAutoMapper(typeof(BLLServiceExtension).Assembly);
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IErrorMapper, ErrorMapper>();
            services.AddScoped<IProductMapper, ProductMapper>();
            services.AddScoped<ICategoryMapper, CategoryMapper>();
            services.AddScoped <IProductManager, ProductManager>();
            services.AddScoped <ICategoryManager, CategoryManager>();
            services.AddScoped<IImageManager, ImageManager>();
            services.AddScoped<ICartManager, CartManager>();
            services.AddScoped<ICartMapper, CartMapper>();
            services.AddScoped<IOrderManager, OrderManager>();
            services.AddScoped<IOrderMapper, OrderMapper>();
            

        }
}
}
