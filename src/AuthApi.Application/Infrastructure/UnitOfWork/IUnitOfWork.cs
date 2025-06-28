namespace AuthApi.Application.Infrastructure.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Task<int> CommitAsync(CancellationToken cancellationToken);
}
