using AuthApi.Application.Features.Users;
using AuthApi.Application.Infrastructure.Data;

namespace AuthApi.Application.Infrastructure.UnitOfWork;

public sealed class UnitOfWork(AuthDbContext context) : IUnitOfWork
{
    private readonly AuthDbContext _context = context;
    private UserRepository _userRepository = null!;

    public UserRepository Users => _userRepository ??= new UserRepository(_context);

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
