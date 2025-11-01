using AuthApi.Infraestructure.Domain;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Infraestructure.Extensions;

public static class DbContextExtensions
{
    public static void MapOnDeleteCascadeRelationships(this ModelBuilder builder)
    {
        foreach (var foreingKey in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            foreingKey.DeleteBehavior = DeleteBehavior.Restrict;
    }

    public static void SeedData(this ModelBuilder modelBuilder, bool includeSeedData)
    {
        if (includeSeedData)
        {
            var userManagerRoleId = Guid.NewGuid();

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = userManagerRoleId, Name = "User Manager", Code = "UM", UpdatedDate = DateTime.UtcNow }
            );

            var userManagerUserId = Guid.NewGuid();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = userManagerUserId,
                    Name = "User Manager",
                    Email = "usermanager@gmail.com",
                    Hash = "$2a$11$uNGxjs/ErX9ro.1SqQKVOeoANXftn18GFpshWP7XjP.fItQKWY7bm",
                    UpdatedDate = DateTime.UtcNow
                }
            );

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { Id = Guid.NewGuid(), IdUser = userManagerUserId, IdRole = userManagerRoleId }
            );
        }
    }
}
