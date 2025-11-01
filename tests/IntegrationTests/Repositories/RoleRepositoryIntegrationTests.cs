using Microsoft.EntityFrameworkCore;
using AuthApi.Application.Persistence.Repositories;
using Tests.IntegrationTests.Base;
using Xunit;

namespace Tests.IntegrationTests.Repositories;

public class RoleRepositoryIntegrationTests : IntegrationTestBase
{
    private readonly RoleRepository _roleRepository;

    public RoleRepositoryIntegrationTests()
    {
        _roleRepository = new RoleRepository(Context);
    }

    [Fact]
    public async Task GetBy_Codes_ReturnsMatchingRoles_WhenRolesExist()
    {
        // Arrange
        await SeedTestRoleDataAsync();

        var codes = new[] { "ADMIN", "USER" };

        // Act
        var result = await _roleRepository.GetBy(codes);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, r => r.Code == "ADMIN");
        Assert.Contains(result, r => r.Code == "USER");
    }

    [Fact]
    public async Task GetBy_Codes_ReturnsEmptyCollection_WhenNoRolesMatch()
    {
        // Arrange
        await SeedTestRoleDataAsync();

        var codes = new[] { "NONEXISTENT", "INVALID" };

        // Act
        var result = await _roleRepository.GetBy(codes);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetBy_Codes_ReturnsPartialMatches_WhenSomeRolesExist()
    {
        // Arrange
        await SeedTestRoleDataAsync();

        var codes = new[] { "ADMIN", "NONEXISTENT", "USER" };

        // Act
        var result = await _roleRepository.GetBy(codes);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, r => r.Code == "ADMIN");
        Assert.Contains(result, r => r.Code == "USER");
    }

    [Fact]
    public async Task GetBy_Ids_ReturnsMatchingRoles_WhenRolesExist()
    {
        // Arrange
        await SeedTestRoleDataAsync();

        var adminRole = await Context.Roles.FirstAsync(r => r.Code == "ADMIN");
        var userRole = await Context.Roles.FirstAsync(r => r.Code == "USER");
        var ids = new[] { adminRole.Id, userRole.Id };

        // Act
        var result = await _roleRepository.GetBy(ids);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, r => r.Id == adminRole.Id);
        Assert.Contains(result, r => r.Id == userRole.Id);
    }

    [Fact]
    public async Task GetBy_Ids_ReturnsEmptyCollection_WhenNoRolesMatch()
    {
        // Arrange
        await SeedTestRoleDataAsync();

        var ids = new[] { Guid.NewGuid(), Guid.NewGuid() };

        // Act
        var result = await _roleRepository.GetBy(ids);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetBy_Ids_ReturnsPartialMatches_WhenSomeRolesExist()
    {
        // Arrange
        await SeedTestRoleDataAsync();

        var adminRole = await Context.Roles.FirstAsync(r => r.Code == "ADMIN");
        var nonExistentId = Guid.NewGuid();
        var ids = new[] { adminRole.Id, nonExistentId };

        // Act
        var result = await _roleRepository.GetBy(ids);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(adminRole.Id, result.First().Id);
    }

    [Fact]
    public async Task GetBy_Codes_WithEmptyCollection_ReturnsEmptyResult()
    {
        // Arrange
        await SeedTestRoleDataAsync();

        var codes = Array.Empty<string>();

        // Act
        var result = await _roleRepository.GetBy(codes);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetBy_Ids_WithEmptyCollection_ReturnsEmptyResult()
    {
        // Arrange
        await SeedTestRoleDataAsync();

        var ids = Array.Empty<Guid>();

        // Act
        var result = await _roleRepository.GetBy(ids);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetBy_Codes_WithCancellationToken_CancelsOperation()
    {
        // Arrange
        await SeedTestRoleDataAsync();

        var codes = new[] { "ADMIN", "USER" };
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            () => _roleRepository.GetBy(codes, cancellationTokenSource.Token));
    }

    [Fact]
    public async Task GetBy_Ids_WithCancellationToken_CancelsOperation()
    {
        // Arrange
        await SeedTestRoleDataAsync();

        var adminRole = await Context.Roles.FirstAsync(r => r.Code == "ADMIN");
        var ids = new[] { adminRole.Id };
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            () => _roleRepository.GetBy(ids, cancellationTokenSource.Token));
    }
}
