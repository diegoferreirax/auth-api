using AuthApi.Application.Infrastructure.Persistence;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuthApi.Application.Features.Users;

public sealed class UserRepository
{
    private readonly IMongoCollection<User> _usersCollection;

    public UserRepository(
        MongoDBDatabaseConfig<User> baseConfig,
        IOptions<MongoDBDatabaseSettings> databaseSettings)
    {
        _usersCollection = baseConfig.GetCollection(databaseSettings.Value.DatabaseCollections.UsersCollection);
    }

    public async Task<Maybe<User>> Get(string email, string password)
    {
        return await _usersCollection.Find(x => x.Email.ToLower() == email.ToLower() && x.Password == password).FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<bool> Exists(string email)
    {
        return await _usersCollection.CountDocumentsAsync(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase)) > 0;
    }

    public async Task Insert(User user)
    {
        await _usersCollection.InsertOneAsync(user).ConfigureAwait(false);
    }
}
