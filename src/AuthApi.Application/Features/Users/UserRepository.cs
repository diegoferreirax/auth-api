using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using AuthApi.Application.Infrastructure.Data;

namespace AuthApi.Application.Features.Users;

public sealed class UserRepository
{
    private readonly AuthDbContext _authDbContext;

    public UserRepository(AuthDbContext authDbContext)
    {
        _authDbContext = authDbContext;
    }

    public async Task<Maybe<User>> GetBy(string email, CancellationToken cancellationToken = default)
    {
        return await _authDbContext.Users
            .Include("Roles")
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email.Equals(email), cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<Maybe<User>> GetBy(Guid id, CancellationToken cancellationToken = default)
    {
        return await _authDbContext.Users
            .Include("Roles")
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
        var count = await _authDbContext.Users.CountAsync();
        var users = await _authDbContext.Users
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .OrderBy(u => u.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return (users, count);
    }

    public async Task Insert(User user, CancellationToken cancellationToken = default)
    {
        await _authDbContext.Users.AddAsync(user).ConfigureAwait(false);
    }

    public void Delete(User user)
    {
        _authDbContext.Users.Remove(user);
    }
}
