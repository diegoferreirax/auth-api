using Microsoft.EntityFrameworkCore;

namespace AuthApi.Infraestructure.Extensions;

public static class DbContextExtensions
{
    public static void MapOnDeleteRestrictRelationships(this ModelBuilder builder)
    {
        foreach (var foreingKey in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            foreingKey.DeleteBehavior = DeleteBehavior.Restrict;
    }
}
