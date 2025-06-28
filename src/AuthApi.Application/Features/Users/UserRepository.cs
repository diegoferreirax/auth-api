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

    public async Task<Maybe<User>> GetBy(string email)
    {
        return await _authDbContext.Users
            .Include("Roles")
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email.Equals(email))
            .ConfigureAwait(false);
    }

    public async Task<Maybe<User>> GetBy(Guid id)
    {
        return await _authDbContext.Users
            .Include("Roles")
            .FirstOrDefaultAsync(x => x.Id.Equals(id))
            .ConfigureAwait(false);
    }

    public async Task<bool> Exists(string email)
    {
        return await _authDbContext.Users
            .AsNoTracking()
            .CountAsync(c => c.Email.Equals(email))
            .ConfigureAwait(false) > 0;
    }

    public async Task Insert(User user)
    {
        await _authDbContext.Users.AddAsync(user).ConfigureAwait(false);
    }

    public void Delete(User user)
    {
        _authDbContext.Users.Remove(user);
    }
}
