using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuthApi.Application.DatabaseContext;

public class MongoDBBaseConfig<T> where T : class
{
    private readonly IMongoDatabase _database;

    public MongoDBBaseConfig(IOptions<AuthDatabaseSettings> bookStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(bookStoreDatabaseSettings.Value.ConnectionString);
        _database = mongoClient.GetDatabase(bookStoreDatabaseSettings.Value.DatabaseName);
    }

    public IMongoCollection<T> GetCollection(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}
