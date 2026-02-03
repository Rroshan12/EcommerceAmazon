using Catalog.Entities;
using MongoDB.Driver;
using Catalog.Dtos;
using Catalog.Extensions;

namespace Catalog.Repository
{
    public interface IBrandRepository
    {
        Task<IEnumerable<ProductBrandDto>> GetAllBrands();
        Task<ProductBrandDto> GetByIdAsync(string id);
        Task<ProductBrandDto> CreateBrand(ProductBrandDto brand);
        Task<ProductBrandDto> UpdateBrand(ProductBrandDto brand);
    }

    public class BrandRepository : IBrandRepository
    {
        private readonly IMongoCollection<ProductBrand> _brands;

        public BrandRepository(IConfiguration config)
        {
            var client = new MongoClient(config["DatabaseSettings:ConnectionString"]);
            var db = client.GetDatabase(config["DatabaseSettings:DatabaseName"]);
            _brands = db.GetCollection<ProductBrand>(config["DatabaseSettings:BrandCollectionName"]);
        }

        public async Task<IEnumerable<ProductBrandDto>> GetAllBrands()
        {
            var entities = await _brands.Find(_ => true).ToListAsync();
            return entities.ToDto();
        }

        public async Task<ProductBrandDto> CreateBrand(ProductBrandDto brandDto)
        {
            var entity = brandDto.ToEntity();
            await _brands.InsertOneAsync(entity);
            return entity.ToDto();
        }

        public async Task<ProductBrandDto> UpdateBrand(ProductBrandDto brandDto)
        {
            var entity = brandDto.ToEntity();
            var filter = Builders<ProductBrand>.Filter.Eq(b => b.Id, entity.Id);
            await _brands.ReplaceOneAsync(filter, entity);
            return entity.ToDto();
        }

        public async Task<ProductBrandDto> GetByIdAsync(string id)
        {
            var filter = Builders<ProductBrand>.Filter.Eq(b => b.Id, id);
            var entity = await _brands.Find(filter).FirstOrDefaultAsync();
            return entity?.ToDto();
        }
    }
}
