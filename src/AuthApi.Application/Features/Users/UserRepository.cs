using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using AuthApi.Application.Persistence.Data;

namespace AuthApi.Application.Features.Users;

public sealed class UserRepository(AuthDbContext authDbContext)
{
    private readonly AuthDbContext _authDbContext = authDbContext;

    public async Task<Maybe<User>> GetBy(string email, CancellationToken cancellationToken = default)
    {
        return await _authDbContext.Users
            .Include("UserRoles")
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email.Equals(email), cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Maybe<User>> GetBy(Guid id, CancellationToken cancellationToken = default)
    {
        return await _authDbContext.Users
            .Include("UserRoles")
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<bool> Exists(string email, CancellationToken cancellationToken = default)
    {
        return await _authDbContext.Users
            .AsNoTracking()
            .CountAsync(c => c.Email.Equals(email), cancellationToken)
            .ConfigureAwait(false) > 0;
    }

    public async Task<(IEnumerable<User>, int)> GetUsers(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var count = await _authDbContext.Users.CountAsync(cancellationToken);
        var users = await _authDbContext.Users
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .OrderBy(u => u.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return (users, count);
    }

    public async Task<User> Insert(User user, CancellationToken cancellationToken = default)
    {
        await _authDbContext.Users.AddAsync(user, cancellationToken).ConfigureAwait(false);
        return user;
    }

    public void Delete(User user)
    {
        _authDbContext.Users.Remove(user);
    }

    public async Task<IEnumerable<Role>> GetRolesBy(IEnumerable<string> codes, CancellationToken cancellationToken = default)
    {
        return await _authDbContext.Roles.AsNoTracking().Where(r => codes.Contains(r.Code)).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Role>> GetRolesBy(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        return await _authDbContext.Roles.AsNoTracking().Where(r => ids.Contains(r.Id)).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task InsertUserRoles(IEnumerable<UserRole> userRoles, CancellationToken cancellationToken = default)
    {
        await _authDbContext.UserRole.AddRangeAsync(userRoles, cancellationToken).ConfigureAwait(false);
    }
}
