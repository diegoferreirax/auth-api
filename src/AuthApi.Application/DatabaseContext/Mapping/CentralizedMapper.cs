namespace AuthApi.Application.DatabaseContext.Mapping;

public static class CentralizedMapper
{
    public static void RegisterMappings()
    {
        UserMapping.RegisterUserMappings();
        BaseEntityMapping.RegisterBaseEntityMappings();
    }
}
