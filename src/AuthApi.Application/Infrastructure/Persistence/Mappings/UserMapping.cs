﻿using AuthApi.Application.Features.Users;
using MongoDB.Bson.Serialization;

namespace AuthApi.Application.Infrastructure.Persistence.Mappings;

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

                cm.MapMember(p => p.Hash)
                  .SetElementName("hash")
                  .SetIsRequired(true);

                cm.MapMember(p => p.Role)
                  .SetElementName("role")
                  .SetIsRequired(true);
            });
        }
    }
}
