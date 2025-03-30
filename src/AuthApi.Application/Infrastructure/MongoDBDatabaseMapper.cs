using AuthApi.Application.Infrastructure.Mappings;

namespace AuthApi.Application.Infrastructure;

public static class MongoDBDatabaseMapper
{
    public static void RegisterMappings()
    {
        UserMapping.RegisterUserMappings();
        BaseEntityMapping.RegisterBaseEntityMappings();
    }
}
