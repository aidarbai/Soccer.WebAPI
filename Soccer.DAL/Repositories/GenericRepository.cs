using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Soccer.DAL.Attributes;
using Soccer.DAL.Models;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : Document
    {
        protected readonly IMongoCollection<T> _collection;
        protected readonly IMongoQueryable<T> _query;
        private readonly IConfiguration _configuration;

        public GenericRepository(IConfiguration configuration)
        {

            _configuration = configuration;

            var mongoClient = new MongoClient(_configuration["Database:ConnectionString"]);

            var mongoDatabase = mongoClient.GetDatabase(_configuration["Database:DatabaseNameFootball"]);

            _collection = mongoDatabase.GetCollection<T>(GenericRepository<T>.GetCollectionName(typeof(T)));

            _query = mongoDatabase.GetCollection<T>(GenericRepository<T>.GetCollectionName(typeof(T))).AsQueryable();
        }

        private protected static string? GetCollectionName(Type documentType)
        {
            return (documentType
                     .GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                     .FirstOrDefault() as BsonCollectionAttribute)?.CollectionName;
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _collection.Find(Builders<T>.Filter.Empty).ToListAsync();

        public async Task<T> GetByIdAsync(string id) => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(T newDocument)
        {
            await _collection.InsertOneAsync(newDocument);
        }

        public async Task CreateManyAsync(IEnumerable<T> newDocuments)
        {
            await _collection.InsertManyAsync(newDocuments);
        }

        public async Task UpdateAsync(T updatedDocument) => await _collection.ReplaceOneAsync<T>(x => x.Id == updatedDocument.Id, updatedDocument);

        public async Task RemoveAsync(string id) => await _collection.DeleteOneAsync(x => x.Id == id);
    }
}
