using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuthApi.Application.Infrastructure.Persistence;

public class MongoDBDatabaseConfig<T> where T : class
{
    private readonly IMongoDatabase _database;
    private readonly IMongoClient _client;

    public MongoDBDatabaseConfig(IOptions<MongoDBDatabaseSettings> bookStoreDatabaseSettings, IMongoClient client)
    {
        _client = client;
        _database = _client.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);
    }

    public IMongoCollection<T> GetCollection(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}
