using Microsoft.EntityFrameworkCore;
using AuthApi.Application.Persistence.Context;
using AuthApi.Application.Features.Users;

namespace Tests.IntegrationTests.Base;

public abstract class IntegrationTestBase : IDisposable
{
    protected readonly AuthDbContext Context;
    protected readonly DbContextOptions<AuthDbContext> DbContextOptions;

    protected IntegrationTestBase()
    {
        DbContextOptions = new DbContextOptionsBuilder<AuthDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new AuthDbContext(DbContextOptions);
        Context.Database.EnsureCreated();
    }

    protected async Task<Role> CreateRoleAsync(string name, string code)
    {
        var role = Role.Create(name, code);
        Context.Roles.Add(role);
        await Context.SaveChangesAsync();
        return role;
    }

    protected async Task SeedTestRoleDataAsync()
    {
        await CreateRoleAsync("Administrator", "ADMIN");
        await CreateRoleAsync("User", "USER");
        await CreateRoleAsync("Manager", "MANAGER");
    }

    protected async Task<User> CreateUserAsync(string name, string email, string password = "password123")
    {
        var userResult = User.Create(name, email);
        if (userResult is null)
            throw new InvalidOperationException($"Failed to create user");

        var user = userResult;
        user.SetHash(password);

        Context.Users.Add(user);
        await Context.SaveChangesAsync();
        return user;
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}
