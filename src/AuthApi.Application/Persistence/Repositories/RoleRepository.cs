using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using AuthApi.Application.Persistence.Context;
using AuthApi.Application.Features.Users;

namespace AuthApi.Application.Persistence.Repositories;

public sealed class RoleRepository(AuthDbContext authDbContext)
{
    private readonly AuthDbContext _authDbContext = authDbContext;

    public async Task<IEnumerable<Role>> GetBy(IEnumerable<string> codes, CancellationToken cancellationToken = default)
    {
        return await _authDbContext.Roles.AsNoTracking().Where(r => codes.Contains(r.Code)).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Role>> GetBy(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        return await _authDbContext.Roles.AsNoTracking().Where(r => ids.Contains(r.Id)).ToListAsync(cancellationToken).ConfigureAwait(false);
    }
}
