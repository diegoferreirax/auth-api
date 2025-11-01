using Microsoft.EntityFrameworkCore;
using AuthApi.Application.Persistence.Repositories;
using Tests.IntegrationTests.Base;
using Xunit;
using AuthApi.Domain.Entities;

namespace Tests.IntegrationTests.Repositories;

public class UserRoleRepositoryIntegrationTests : IntegrationTestBase
{
    private readonly UserRoleRepository _userRoleRepository;

    public UserRoleRepositoryIntegrationTests()
    {
        _userRoleRepository = new UserRoleRepository(Context);
    }

    [Fact]
    public async Task Insert_SingleUserRole_AddsToDatabase()
    {
        // Arrange
        var user = await CreateUserAsync("Test User", "test@example.com");
        var role = await CreateRoleAsync("Test Role", "TEST");
        var userRole = UserRole.Create(user.Id, role.Id);

        // Act
        await _userRoleRepository.Insert(new[] { userRole });
        await Context.SaveChangesAsync();

        // Assert
        var savedUserRole = await Context.UserRole
            .FirstOrDefaultAsync(ur => ur.IdUser == user.Id && ur.IdRole == role.Id);
        
        Assert.NotNull(savedUserRole);
        Assert.Equal(user.Id, savedUserRole.IdUser);
        Assert.Equal(role.Id, savedUserRole.IdRole);
    }

    [Fact]
    public async Task Insert_MultipleUserRoles_AddsAllToDatabase()
    {
        // Arrange
        var user1 = await CreateUserAsync("User 1", "user1@example.com");
        var user2 = await CreateUserAsync("User 2", "user2@example.com");
        var role1 = await CreateRoleAsync("Role 1", "ROLE1");
        var role2 = await CreateRoleAsync("Role 2", "ROLE2");

        var userRole1 = UserRole.Create(user1.Id, role1.Id);
        var userRole2 = UserRole.Create(user1.Id, role2.Id);
        var userRole3 = UserRole.Create(user2.Id, role1.Id);
        
        var userRoles = new[]
        {
            userRole1,
            userRole2,
            userRole3
        };

        // Act
        await _userRoleRepository.Insert(userRoles);
        await Context.SaveChangesAsync();

        // Assert
        var savedUserRoles = await Context.UserRole.ToListAsync();
        Assert.Equal(3, savedUserRoles.Count);
        
        Assert.Contains(savedUserRoles, ur => ur.IdUser == user1.Id && ur.IdRole == role1.Id);
        Assert.Contains(savedUserRoles, ur => ur.IdUser == user1.Id && ur.IdRole == role2.Id);
        Assert.Contains(savedUserRoles, ur => ur.IdUser == user2.Id && ur.IdRole == role1.Id);
    }

    [Fact]
    public async Task Insert_EmptyCollection_DoesNotThrowException()
    {
        // Arrange
        var userRoles = Array.Empty<UserRole>();

        // Act & Assert
        await _userRoleRepository.Insert(userRoles);
        await Context.SaveChangesAsync();

        var count = await Context.UserRole.CountAsync();
        Assert.Equal(0, count);
    }

    [Fact]
    public async Task Insert_WithCancellationToken_CompletesSuccessfully()
    {
        // Arrange
        var user = await CreateUserAsync("Test User", "test@example.com");
        var role = await CreateRoleAsync("Test Role", "TEST");
        var userRole = UserRole.Create(user.Id, role.Id);

        var cancellationTokenSource = new CancellationTokenSource();

        // Act
        await _userRoleRepository.Insert(new[] { userRole }, cancellationTokenSource.Token);
        await Context.SaveChangesAsync();

        // Assert
        var savedUserRole = await Context.UserRole
            .FirstOrDefaultAsync(ur => ur.IdUser == user.Id && ur.IdRole == role.Id);
        Assert.NotNull(savedUserRole);
    }
}
