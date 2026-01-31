using AuthApi.Application.Persistence.Context;
using AuthApi.Domain.Entities;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Application.Persistence.Repositories;

public sealed class UserRepository(AuthDbContext authDbContext)
{
    private readonly AuthDbContext _authDbContext = authDbContext;

    public async Task<Maybe<User>> GetBy(string email, CancellationToken cancellationToken = default)
    {
        return await _authDbContext.Users
            .Include(u => u.UserRoles)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email.Equals(email), cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Maybe<User>> GetBy(Guid id, CancellationToken cancellationToken = default)
    {
        return await _authDbContext.Users
            .Include(u => u.UserRoles)
            .AsNoTracking()
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
        var count = await _authDbContext.Users
            .AsNoTracking()
            .CountAsync(cancellationToken)
            .ConfigureAwait(false);

        var users = await _authDbContext.Users
            .Include(u => u.UserRoles)
            .AsNoTracking()
            .OrderBy(u => u.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return (users, count);
    }

    public void Insert(User user)
    {
        _authDbContext.Users.Add(user);
    }

    public void Update(User user)
    {
        _authDbContext.Users.Update(user);
    }

    public void Delete(User user)
    {
        _authDbContext.Users.Remove(user);
    }
}
