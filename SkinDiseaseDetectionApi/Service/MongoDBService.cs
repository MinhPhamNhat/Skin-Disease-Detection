using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using SkinDiseaseDetectionApi.Models;

namespace SkinDiseaseDetectionApi.Service
{
    public class MongoDBService
    {
        private readonly IMongoDatabase database;

        public MongoDBService(IOptions<MongoDBSetting> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        }

        public async Task<List<T>> GetAsync<T>()
        {
            var collection = database.GetCollection<T>(typeof(T).Name);
            return await collection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task CreateAsync<T>(T obj)
        {
            var collection = database.GetCollection<T>(typeof(T).Name);
            await collection.InsertOneAsync(obj);
        }

        public async Task<List<T>> FindAsync<T>(Expression<Func<T, bool>> predicate)
        {
            var collection = database.GetCollection<T>(typeof(T).Name);
            return await collection.Find(predicate).ToListAsync();
        }

        public async Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate)
        {
            var collection = database.GetCollection<T>(typeof(T).Name);
            return await collection.Find(predicate).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync<T>(string id, T obj)
        {
            var collection = database.GetCollection<T>(typeof(T).Name);
            var filter = Builders<T>.Filter.Eq("_id", id);
            await collection.ReplaceOneAsync(filter, obj);
        }

        public async Task<T> FindByIdAsync<T>(int id)
        {
            var collection = database.GetCollection<T>(typeof(T).Name);
            var filter = Builders<T>.Filter.Eq("_id", id);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}