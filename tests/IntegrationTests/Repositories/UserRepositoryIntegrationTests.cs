using AuthApi.Application.Persistence.Repositories;
using Xunit;
using Assert = Xunit.Assert;
using Tests.IntegrationTests.Base;

namespace Tests.IntegrationTests.Repositories;

public class UserRepositoryIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task GetBy_Email_ReturnsUser_WhenUserExists()
    {
        // Arrange
        var email = "test@example.com";
        var user = await CreateUserAsync("Test", email, "pass");
        var repo = new UserRepository(Context);

        // Act
        var result = await repo.GetBy(email);

        // Assert
        Assert.True(result.HasValue);
        Assert.Equal(email, result.Value.Email);
    }

    [Fact]
    public async Task GetBy_Email_ReturnsNone_WhenUserDoesNotExist()
    {
        // Arrange
        var email = "notfound@example.com";
        var repo = new UserRepository(Context);

        // Act
        var result = await repo.GetBy(email);

        // Assert
        Assert.True(result.HasNoValue);
    }

    [Fact]
    public async Task GetBy_Id_ReturnsUser_WhenUserExists()
    {
        // Arrange
        var user = await CreateUserAsync("Test", "test2@example.com", "pass");
        var repo = new UserRepository(Context);

        // Act
        var result = await repo.GetBy(user.Id);

        // Assert
        Assert.True(result.HasValue);
        Assert.Equal(user.Id, result.Value.Id);
    }

    [Fact]
    public async Task GetBy_Id_ReturnsNone_WhenUserDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        var repo = new UserRepository(Context);

        // Act
        var result = await repo.GetBy(id);

        // Assert
        Assert.True(result.HasNoValue);
    }

    [Fact]
    public async Task Exists_WithExistingEmail_ReturnsTrue()
    {
        // Arrange
        var email = "test@example.com";
        var user = await CreateUserAsync("Test User", email);
        var repo = new UserRepository(Context);

        // Act
        var result = await repo.Exists(email);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Exists_WithNonExistingEmail_ReturnsFalse()
    {
        // Arrange
        var email = "nonexistent@example.com";
        var repo = new UserRepository(Context);

        // Act
        var result = await repo.Exists(email);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Exists_WithCaseSensitiveEmail_ReturnsFalse()
    {
        // Arrange
        var email = "test@example.com";
        var upperCaseEmail = "TEST@EXAMPLE.COM";
        await CreateUserAsync("Test User", email);
        var repo = new UserRepository(Context);

        // Act
        var result = await repo.Exists(upperCaseEmail);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Exists_WithNullEmail_ReturnsFalse()
    {
        // Arrange
        string? email = null;
        var repo = new UserRepository(Context);

        // Act
        var result = await repo.Exists(email!);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Exists_WithEmptyEmail_ReturnsFalse()
    {
        // Arrange
        var email = string.Empty;
        var repo = new UserRepository(Context);

        // Act
        var result = await repo.Exists(email);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Exists_WithWhitespaceEmail_ReturnsFalse()
    {
        // Arrange
        var email = "   ";
        await CreateUserAsync("Test User", "test@example.com");
        var repo = new UserRepository(Context);

        // Act
        var result = await repo.Exists(email);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Exists_WithCancellationToken_ExecutesSuccessfully()
    {
        // Arrange
        var email = "test@example.com";
        await CreateUserAsync("Test User", email);
        var cancellationToken = new CancellationToken();
        var repo = new UserRepository(Context);

        // Act
        var result = await repo.Exists(email, cancellationToken);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task Exists_WithCancelledToken_ThrowsOperationCanceledException()
    {
        // Arrange
        var email = "test@example.com";
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();
        var cancelledToken = cancellationTokenSource.Token;
        var repo = new UserRepository(Context);

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(
            () => repo.Exists(email, cancelledToken));
    }

    [Fact]
    public async Task Exists_WithMultipleUsers_ReturnsTrueForExistingEmail()
    {
        // Arrange
        var repo = new UserRepository(Context);
        var email1 = "user1@example.com";
        var email2 = "user2@example.com";
        var email3 = "user3@example.com";

        await CreateUserAsync("User 1", email1);
        await CreateUserAsync("User 2", email2);
        await CreateUserAsync("User 3", email3);

        // Act
        var result1 = await repo.Exists(email1);
        var result2 = await repo.Exists(email2);
        var result3 = await repo.Exists(email3);
        var resultNonExistent = await repo.Exists("nonexistent@example.com");

        // Assert
        Assert.True(result1);
        Assert.True(result2);
        Assert.True(result3);
        Assert.False(resultNonExistent);
    }
}
