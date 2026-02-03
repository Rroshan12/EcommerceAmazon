using Catalog.Entities;
using MongoDB.Driver;
using Catalog.Dtos;
using Catalog.Extensions;

namespace Catalog.Repository
{
    public interface ITypeRepository
    {
        Task<IEnumerable<ProductTypeDto>> GetAllTypes();

        Task<ProductTypeDto> GetByIdAsync(string id);
    }
    public class TypeRepository : ITypeRepository
    {
        private readonly IMongoCollection<ProductType> _types;

        public TypeRepository(IConfiguration config)
        {
            var client = new MongoClient(config["DatabaseSettings:ConnectionString"]);
            var db = client.GetDatabase(config["DatabaseSettings:DatabaseName"]);
            _types = db.GetCollection<ProductType>(config["DatabaseSettings:TypeCollectionName"]);
        }

        public async Task<IEnumerable<ProductTypeDto>> GetAllTypes()
        {
            var entities = await _types.Find(_ => true).ToListAsync();
            return entities.Select(t => t.ToDto());
        }
        public async Task<ProductTypeDto> GetByIdAsync(string id)
        {
            var filter = Builders<ProductType>.Filter.Eq(b => b.Id, id);
            var entity = await _types.Find(filter).FirstOrDefaultAsync();
            return entity?.ToDto();
        }
    }
}
