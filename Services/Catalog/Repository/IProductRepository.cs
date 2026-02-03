using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Specifications;
using MongoDB.Driver;
using Catalog.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Catalog.Repository
{
    public interface IProductRepository
    {
        Task<PaginationDto<ProductDto>> GetAllAsync(CatalogSpecParams specParams);
        Task<IEnumerable<ProductDto>> GetProductsByNameAsync(string name);

        Task<IEnumerable<ProductDto>> GetProductsByBrandNameAsync(string name);

        Task<ProductDto> GetProduct(string productId);

        Task<ProductDto> CreateProduct(ProductDto product);

        Task<ProductDto> UpdateProduct(ProductDto product);
        Task<ProductDto> DeleteProduct(string productId);
        Task<ProductDto> GetProductByBrandId(string brandId);

        Task<ProductDto> GetProductByTypeId(string typeId);

    }


        public class ProductRepository : IProductRepository
        {
            private readonly IMongoCollection<Product> _products;

            public ProductRepository(IConfiguration config)
            {
                var client = new MongoClient(config["DatabaseSettings:ConnectionString"]);
                var database = client.GetDatabase(config["DatabaseSettings:DatabaseName"]);
                _products = database.GetCollection<Product>(
                    config["DatabaseSettings:ProductCollectionName"]);
            }

        public async Task<PaginationDto<ProductDto>> GetAllAsync(CatalogSpecParams specParams)
        {
            var filterBuilder = Builders<Product>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrEmpty(specParams.BrandId))
                filter &= filterBuilder.Eq(p => p.BrandId, specParams.BrandId);

            if (!string.IsNullOrEmpty(specParams.TypeId))
                filter &= filterBuilder.Eq(p => p.TypeId, specParams.TypeId);

            if (!string.IsNullOrEmpty(specParams.Search))
                filter &= filterBuilder.Regex(
                    p => p.Name,
                    new MongoDB.Bson.BsonRegularExpression(specParams.Search, "i"));

            var totalCount = await _products.CountDocumentsAsync(filter);

            var data = await _products.Find(filter)
                .Skip((specParams.PageNumber - 1) * specParams.PageSize)
                .Limit(specParams.PageSize)
                .ToListAsync();

            var pagination = new Catalog.Specifications.Pagination<Product>(
                specParams.PageNumber,
                specParams.PageSize,
                (int)totalCount,
                data
            );

            return pagination.ToDto(p => p.ToDto());
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByNameAsync(string name)
            {
                var filter = Builders<Product>.Filter
                    .Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));

                var entities = await _products.Find(filter).ToListAsync();
                return entities.Select(e => e.ToDto());
            }

            public async Task<IEnumerable<ProductDto>> GetProductsByBrandNameAsync(string name)
            {
                var filter = Builders<Product>.Filter
                    .Regex(p => p.Brand.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));

                var entities = await _products.Find(filter).ToListAsync();
                return entities.Select(e => e.ToDto());
            }

            public async Task<ProductDto> GetProduct(string productId)
            {
                var entity = await _products
                    .Find(p => p.Id == productId)
                    .FirstOrDefaultAsync();

                return entity?.ToDto();
            }

            public async Task<ProductDto> CreateProduct(ProductDto product)
            {
                var entity = product.ToEntity();
                await _products.InsertOneAsync(entity);
                return entity.ToDto();
            }

            public async Task<ProductDto> UpdateProduct(ProductDto product)
            {
                var entity = product.ToEntity();
                var filter = Builders<Product>.Filter.Eq(p => p.Id, entity.Id);

                var result = await _products.ReplaceOneAsync(filter, entity);

            return entity.ToDto();
            }

            public async Task<ProductDto> DeleteProduct(string productId)
            {
                var entity = await _products
                    .FindOneAndDeleteAsync(p => p.Id == productId);

                return entity?.ToDto();
            }

            public async Task<ProductDto> GetProductByBrandId(string brandId)
            {
                var entity = await _products
                    .Find(p => p.Brand.Id == brandId)
                    .FirstOrDefaultAsync();

                return entity?.ToDto();
            }

            public async Task<ProductDto> GetProductByTypeId(string typeId)
            {
                var entity = await _products
                    .Find(p => p.TypeId == typeId)
                    .FirstOrDefaultAsync();

                return entity?.ToDto();
            }
        }
    }


