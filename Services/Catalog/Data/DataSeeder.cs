using Catalog.Entities;
using MongoDB.Driver;
using System.Text.Json;
using System.IO;

namespace Catalog.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["DatabaseSettings:ConnectionString"]);
            var db = client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);

            var brands = db.GetCollection<ProductBrand>(configuration["DatabaseSettings:BrandCollectionName"]);
            var types = db.GetCollection<ProductType>(configuration["DatabaseSettings:TypeCollectionName"]);
            var products = db.GetCollection<Product>(configuration["DatabaseSettings:ProductCollectionName"]);

            // try load seed files from output directory
            var basePath = AppContext.BaseDirectory;
            var seedDir = Path.Combine(basePath, "Data", "seed");

            // Seed brands
            if ((await brands.CountDocumentsAsync(FilterDefinition<ProductBrand>.Empty)) == 0)
            {
                var brandsFile = Path.Combine(seedDir, "brands.json");
                if (File.Exists(brandsFile))
                {
                    var json = await File.ReadAllTextAsync(brandsFile);
                    var seedBrands = JsonSerializer.Deserialize<List<ProductBrand>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ProductBrand>();
                    if (seedBrands.Any()) await brands.InsertManyAsync(seedBrands);
                }
                else
                {
                    var seedBrands = new[] {
                        new ProductBrand { Name = "Acme" },
                        new ProductBrand { Name = "Contoso" },
                        new ProductBrand { Name = "Fabrikam" }
                    };
                    await brands.InsertManyAsync(seedBrands);
                }
            }

            // Seed types
            if ((await types.CountDocumentsAsync(FilterDefinition<ProductType>.Empty)) == 0)
            {
                var typesFile = Path.Combine(seedDir, "types.json");
                if (File.Exists(typesFile))
                {
                    var json = await File.ReadAllTextAsync(typesFile);
                    var seedTypes = JsonSerializer.Deserialize<List<ProductType>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ProductType>();
                    if (seedTypes.Any()) await types.InsertManyAsync(seedTypes);
                }
                else
                {
                    var seedTypes = new[] {
                        new ProductType { Name = "Smartphone" },
                        new ProductType { Name = "Laptop" },
                        new ProductType { Name = "Headphones" }
                    };
                    await types.InsertManyAsync(seedTypes);
                }
            }

            // Seed products (products.json can reference brandName and typeName)
            if ((await products.CountDocumentsAsync(FilterDefinition<Product>.Empty)) == 0)
            {
                var productsFile = Path.Combine(seedDir, "products.json");
                if (File.Exists(productsFile))
                {
                    var json = await File.ReadAllTextAsync(productsFile);
                    var items = JsonSerializer.Deserialize<List<ProductSeedItem>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ProductSeedItem>();

                    var existingBrands = await brands.Find(FilterDefinition<ProductBrand>.Empty).ToListAsync();
                    var existingTypes = await types.Find(FilterDefinition<ProductType>.Empty).ToListAsync();

                    var seedProducts = new List<Product>();
                    foreach (var it in items)
                    {
                        var brand = existingBrands.FirstOrDefault(b => string.Equals(b.Name, it.BrandName, StringComparison.OrdinalIgnoreCase)) ?? existingBrands.FirstOrDefault();
                        var type = existingTypes.FirstOrDefault(t => string.Equals(t.Name, it.TypeName, StringComparison.OrdinalIgnoreCase)) ?? existingTypes.FirstOrDefault();

                        seedProducts.Add(new Product
                        {
                            Name = it.Name,
                            Description = it.Description,
                            Summary = it.Summary,
                            ImageFile = it.ImageFile,
                            BrandId = brand?.Id,
                            Brand = brand,
                            TypeId = type?.Id,
                            Type = type,
                            Price = it.Price,
                            CreatedDate = DateTimeOffset.UtcNow
                        });
                    }

                    if (seedProducts.Any()) await products.InsertManyAsync(seedProducts);
                }
                else
                {
                    // fallback inline products
                    var firstBrand = await brands.Find(FilterDefinition<ProductBrand>.Empty).FirstOrDefaultAsync();
                    var firstType = await types.Find(FilterDefinition<ProductType>.Empty).FirstOrDefaultAsync();

                    var seedProducts = new[] {
                        new Product { Name = "Acme Phone", Description = "A phone", Summary = "Phone", ImageFile = "phone.jpg", BrandId = firstBrand?.Id, Brand = firstBrand, TypeId = firstType?.Id, Type = firstType, Price = 499.99m, CreatedDate = DateTimeOffset.UtcNow },
                        new Product { Name = "Contoso Laptop", Description = "A laptop", Summary = "Laptop", ImageFile = "laptop.jpg", BrandId = firstBrand?.Id, Brand = firstBrand, TypeId = firstType?.Id, Type = firstType, Price = 999.99m, CreatedDate = DateTimeOffset.UtcNow }
                    };

                    await products.InsertManyAsync(seedProducts);
                }
            }
        }

        private class ProductSeedItem
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Summary { get; set; }
            public string ImageFile { get; set; }
            public string BrandName { get; set; }
            public string TypeName { get; set; }
            public decimal Price { get; set; }
        }
    }
}
