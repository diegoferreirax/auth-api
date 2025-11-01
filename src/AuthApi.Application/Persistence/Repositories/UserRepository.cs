using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using AuthApi.Application.Persistence.Context;
using AuthApi.Application.Features.Users;

namespace AuthApi.Application.Persistence.Repositories;

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
            .Include("UserRoles")
            .OrderBy(u => u.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return (users, count);
    }

    public async Task<User> Insert(User user, CancellationToken cancellationToken = default)
    {
        await _authDbContext.Users.AddAsync(user, cancellationToken).ConfigureAwait(false);
        return user;
    }

    public User Update(User user)
    {
        _authDbContext.Users.Update(user);
        return user;
    }

    public void Delete(User user)
    {
        _authDbContext.Users.Remove(user);
    }
}
