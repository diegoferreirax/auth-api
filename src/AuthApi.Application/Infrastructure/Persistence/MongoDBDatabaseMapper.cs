using AuthApi.Application.Infrastructure.Persistence.Mappings;

namespace AuthApi.Application.Infrastructure.Persistence;

public static class MongoDBDatabaseMapper
{
    public static void RegisterMappings()
    {
        UserMapping.RegisterUserMappings();
        BaseEntityMapping.RegisterBaseEntityMappings();
    }
}
