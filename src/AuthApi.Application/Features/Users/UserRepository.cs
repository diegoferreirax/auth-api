using AuthApi.Application.DatabaseContext;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuthApi.Application.Features.Users;

public sealed class UserRepository
{
    private readonly IMongoCollection<User> _usersCollection;

    public UserRepository(
        MongoDBBaseConfig<User> baseConfig,
        IOptions<AuthDatabaseSettings> databaseSettings)
    {
        _usersCollection = baseConfig.GetCollection(databaseSettings.Value.DatabaseCollections.UsersCollection);
    }

    public async Task<Maybe<User>> Get(string username, string password)
    {
        return await _usersCollection.Find(x => x.Name.ToLower() == username.ToLower() && x.Password == password).FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<bool> Exists(string username)
    {
        return await _usersCollection.CountDocumentsAsync(x => x.Name.Equals(username, StringComparison.OrdinalIgnoreCase)) > 0;
    }

    public async Task Insert(User user)
    {
        await _usersCollection.InsertOneAsync(user).ConfigureAwait(false);
    }
}
