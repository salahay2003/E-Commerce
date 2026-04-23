namespace ECommerce.DAL
{
    public static class SeedDataProvider
    {
        public static List<Product> GetProducts()
        {
            var createdDate = new DateTime(2026, 3, 1, 10, 30, 0);
            return new List<Product>
            {
            new Product { Name = "iPhone 14", Price = 1200m, StockQty = 10, CategoryId = 1, CreatedAt = createdDate },
            new Product { Name = "Samsung TV", Price = 800m, StockQty = 5, CategoryId = 1, CreatedAt = createdDate },
            new Product { Name = "Laptop Dell XPS", Price = 1500m, StockQty = 7, CategoryId = 1, CreatedAt = createdDate },

            new Product { Name = "T-Shirt", Price = 20m, StockQty = 50, CategoryId = 2, CreatedAt = createdDate },
            new Product { Name = "Jeans", Price = 40m, StockQty = 30, CategoryId = 2, CreatedAt = createdDate },
            new Product { Name = "Jacket", Price = 100m, StockQty = 15, CategoryId = 2, CreatedAt = createdDate },

            new Product { Name = "Clean Code Book", Price = 30m, StockQty = 20, CategoryId = 3, CreatedAt = createdDate },
            new Product { Name = "Design Patterns Book", Price = 45m, StockQty = 12, CategoryId = 3, CreatedAt = createdDate },
            new Product { Name = "Algorithms Book", Price = 60m, StockQty = 8, CategoryId = 3, CreatedAt = createdDate }
            };
        }

        public static List<Category> GetCategories()
        {
            var createdDate = new DateTime(2026, 3, 1, 10, 30, 0);

            return new List<Category>
            {
                new Category {  Name = "Electronics", CreatedAt = DateTime.UtcNow },
                new Category {  Name = "Clothing", CreatedAt = DateTime.UtcNow },
                new Category {  Name = "Books", CreatedAt = DateTime.UtcNow }
            };
        }
    }
}
