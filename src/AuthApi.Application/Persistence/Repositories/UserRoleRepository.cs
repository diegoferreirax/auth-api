using AuthApi.Application.Persistence.Context;
using AuthApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Application.Persistence.Repositories;

public sealed class UserRoleRepository(AuthDbContext authDbContext)
{
    private readonly AuthDbContext _authDbContext = authDbContext;

    public void Insert(IEnumerable<UserRole> userRoles)
    {
        _authDbContext.UserRole.AddRange(userRoles);
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
