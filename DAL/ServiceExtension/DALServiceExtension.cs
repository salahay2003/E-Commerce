using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;
namespace ECommerce.DAL
{
    public static class DALServiceExtension
    {
        public static void AddDALServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ECommerceProject");
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString)
                .UseAsyncSeeding(async (context, _, _) =>
                {
                    if (await context.Set<Product>().AnyAsync())
                    {
                        return;
                    }
                    if (await context.Set<Category>().AnyAsync())
                    {
                        return;
                    }

                    var categories = SeedDataProvider.GetCategories();
                    var products = SeedDataProvider.GetProducts();

                    await context.AddRangeAsync(categories);
                    await context.AddRangeAsync(products);

                    await context.SaveChangesAsync();
                })
                .UseSeeding((context, _) =>
                {
                    if (context.Set<Product>().Any())
                    {
                        return;
                    }
                    if(context.Set<Category>().Any())
                    {
                        return;
                    }
                    var categories = SeedDataProvider.GetCategories();
                    var products = SeedDataProvider.GetProducts();

                     context.AddRange(categories);
                     context.AddRange(products);

                     context.SaveChanges();
                });                
            });

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                options.User.RequireUniqueEmail = false;

                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
            }).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

        }
    }
}
