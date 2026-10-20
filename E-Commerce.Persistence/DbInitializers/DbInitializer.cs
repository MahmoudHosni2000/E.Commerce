using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Persistence.DbInitializers
{
    public class DbInitializer(StoreDbContext context) : IDbInitializer
    {
        public void Initialize()
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var seedDataPath = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "E-Commerce.Persistence", "Context", "DataSeed"));

            if (context.Database.CanConnect())
            {
                if (!context.ProductBrands.Any())
                {
                    // Seed ProductBrands
                    var brandsData = File.ReadAllText(Path.Combine(seedDataPath, "brands.json"));
                    // Configure the JSON serializer to ignore case sensitivity
                    var options = new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    // Deserialize the JSON data into a list of Brand objects
                    var brands = System.Text.Json.JsonSerializer.Deserialize<List<ProductBrand>>(brandsData, options);
                    // Check if the deserialization was successful and if there are any brands to add
                    if (brands != null && brands.Count > 0)
                        context.ProductBrands.AddRange(brands); // Add the list of brands to the database context
                    // Save the changes to the database
                    context.SaveChanges();
                }

                if (!context.ProductTypes.Any())
                {
                    // Seed ProductTypes
                    var typesData = File.ReadAllText(Path.Combine(seedDataPath, "types.json"));
                    var options = new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var types = System.Text.Json.JsonSerializer.Deserialize<List<ProductType>>(typesData, options);
                    if (types != null && types.Count > 0)
                        context.ProductTypes.AddRange(types);
                    context.SaveChanges();
                }

                if (!context.Products.Any())
                {
                    // Seed Products
                    var productsData = File.ReadAllText(Path.Combine(seedDataPath, "products.json"));
                    // Configure the JSON serializer to ignore case sensitivity
                    var options = new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    // Deserialize the JSON data into a list of Brand objects
                    var products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(productsData, options);
                    // Check if the deserialization was successful and if there are any brands to add
                    if (products != null && products.Count > 0)
                        context.Products.AddRange(products); // Add the list of brands to the database context
                    // Save the changes to the database
                    context.SaveChanges();
                }
            }
        }
    }
}
