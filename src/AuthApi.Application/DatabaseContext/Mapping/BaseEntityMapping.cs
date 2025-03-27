using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace AuthApi.Application.DatabaseContext.Mapping;

public static class BaseEntityMapping
{
    public static void RegisterBaseEntityMappings()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(BaseEntity)))
        {
            BsonClassMap.RegisterClassMap<BaseEntity>(cm =>
            {
                cm.MapIdMember(p => p.Id)
                  .SetIdGenerator(GuidGenerator.Instance)
                  .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));

                cm.MapMember(p => p.UpdatedDate)
                  .SetElementName("updatedDate")
                  .SetIsRequired(true);
            });
        }
    }
}
