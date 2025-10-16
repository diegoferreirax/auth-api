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
        var adminRoleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var userRoleId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var masterRoleId = Guid.Parse("33333333-3333-3333-3333-333333333333");

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = masterRoleId, Name = "MASTER", Code = "MASTER", UpdatedDate = DateTime.UtcNow },
            new Role { Id = adminRoleId, Name = "ADMIN", Code = "ADMIN", UpdatedDate = DateTime.UtcNow },
            new Role { Id = userRoleId, Name = "USER", Code = "USER", UpdatedDate = DateTime.UtcNow }
        );

        if (includeSeedData)
        {
            var adminUserId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var userUserId = Guid.Parse("55555555-5555-5555-5555-555555555555");
            var masterId = Guid.Parse("66666666-6666-6666-6666-666666666666");

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = adminUserId,
                    Name = "Administrador",
                    Email = "admin@authapi.com",
                    Hash = "BCrypt.Net.BCrypt.HashPassword(admin123)",
                    UpdatedDate = DateTime.UtcNow
                },
                new User
                {
                    Id = userUserId,
                    Name = "Usuário Padrão",
                    Email = "user@authapi.com",
                    Hash = "BCrypt.Net.BCrypt.HashPassword(admin123)",
                    UpdatedDate = DateTime.UtcNow
                },
                new User
                {
                    Id = masterId,
                    Name = "Gerente",
                    Email = "master@authapi.com",
                    Hash = "BCrypt.Net.BCrypt.HashPassword(admin123)",
                    UpdatedDate = DateTime.UtcNow
                }
            );

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { Id = Guid.Parse("77777777-7777-7777-7777-777777777777"), IdUser = adminUserId, IdRole = adminRoleId },
                new UserRole { Id = Guid.Parse("88888888-8888-8888-8888-888888888888"), IdUser = userUserId, IdRole = userRoleId },
                new UserRole { Id = Guid.Parse("99999999-9999-9999-9999-999999999999"), IdUser = masterId, IdRole = masterRoleId },
                new UserRole { Id = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"), IdUser = masterId, IdRole = userRoleId }
            );
        }
    }
}
