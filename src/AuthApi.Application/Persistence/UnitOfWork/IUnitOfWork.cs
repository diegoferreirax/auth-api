using AuthApi.Application.Features.Users;

namespace AuthApi.Application.Persistence.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    UserRepository Users { get; }
    Task<int> CommitAsync(CancellationToken cancellationToken);
}
