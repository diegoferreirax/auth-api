using AuthApi.Application.Persistence.Data;
using AuthApi.Application.Features.Users;

namespace AuthApi.Application.Persistence.Repositories;

public sealed class UserRoleRepository(AuthDbContext authDbContext)
{
    private readonly AuthDbContext _authDbContext = authDbContext;

    public async Task Insert(IEnumerable<UserRole> userRoles, CancellationToken cancellationToken = default)
    {
        await _authDbContext.UserRole.AddRangeAsync(userRoles, cancellationToken).ConfigureAwait(false);
    }
}
