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

        public async Task<ProductBrandDto> GetByIdAsync(string id)
        {
            var filter = Builders<ProductBrand>.Filter.Eq(b => b.Id, id);
            var entity = await _brands.Find(filter).FirstOrDefaultAsync();
            return entity?.ToDto();
        }
    }
}
