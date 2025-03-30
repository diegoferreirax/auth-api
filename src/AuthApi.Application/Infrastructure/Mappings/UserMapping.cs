using AuthApi.Application.Features.Users;
using MongoDB.Bson.Serialization;

namespace AuthApi.Application.Infrastructure.Mappings;

public static class UserMapping
{
    public static void RegisterUserMappings()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(User)))
        {
            BsonClassMap.RegisterClassMap<User>(cm =>
            {
                cm.MapMember(p => p.Name)
                  .SetElementName("name")
                  .SetIsRequired(true);

                cm.MapMember(p => p.Email)
                  .SetElementName("email")
                  .SetIsRequired(true);

                cm.MapMember(p => p.Password)
                  .SetElementName("password")
                  .SetIsRequired(true);

                cm.MapMember(p => p.Role)
                  .SetElementName("role")
                  .SetIsRequired(true);
            });
        }
    }
}
