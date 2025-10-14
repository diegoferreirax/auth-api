using AuthApi.Application.Persistence.Repositories;

namespace AuthApi.Application.Persistence.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    UserRepository Users { get; }
    RoleRepository Roles { get; }
    UserRoleRepository UserRoles { get; }
    Task<int> CommitAsync(CancellationToken cancellationToken);
}
