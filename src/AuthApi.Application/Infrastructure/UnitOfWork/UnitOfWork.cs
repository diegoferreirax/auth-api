using AuthApi.Application.Infrastructure.Data;

namespace AuthApi.Application.Infrastructure.UnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AuthDbContext _context;

    public UnitOfWork(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
