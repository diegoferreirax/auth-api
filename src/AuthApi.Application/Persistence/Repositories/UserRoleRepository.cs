using AuthApi.Application.Persistence.Context;
using AuthApi.Application.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Application.Persistence.Repositories;

public sealed class UserRoleRepository(AuthDbContext authDbContext)
{
    private readonly AuthDbContext _authDbContext = authDbContext;

    public async Task Insert(IEnumerable<UserRole> userRoles, CancellationToken cancellationToken = default)
    {
        await _authDbContext.UserRole.AddRangeAsync(userRoles, cancellationToken).ConfigureAwait(false);
    }

    public async Task DeleteByUserId(Guid userId, CancellationToken cancellationToken = default)
    {
        var userRoles = await _authDbContext.UserRole
            .Where(ur => ur.IdUser == userId)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        _authDbContext.UserRole.RemoveRange(userRoles);
    }
}
