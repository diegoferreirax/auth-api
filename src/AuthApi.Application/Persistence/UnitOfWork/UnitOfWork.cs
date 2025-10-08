using AuthApi.Application.Persistence.Data;
using AuthApi.Application.Persistence.Repositories;

namespace AuthApi.Application.Persistence.UnitOfWork;

public sealed class UnitOfWork(
    AuthDbContext context,
    UserRepository userRepository,
    RoleRepository roleRepository,
    UserRoleRepository userRoleRepository) : IUnitOfWork
{
    private readonly AuthDbContext _context = context;
    private readonly UserRepository _userRepository = userRepository;
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly UserRoleRepository _userRoleRepository = userRoleRepository;

    public UserRepository Users => _userRepository;
    public RoleRepository Roles => _roleRepository;
    public UserRoleRepository UserRoles => _userRoleRepository;

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
