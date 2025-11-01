using AuthApi.Application.Persistence.UnitOfWork;
using AuthApi.Application.Resource;
using AuthApi.Application.Security.Bcrypt;
using AuthApi.Application.Exceptions;

namespace AuthApi.Application.Features.Users.UpdateUser.v1;

public sealed class UpdateUserHandler(
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
{
    public readonly IPasswordHasher _passwordHasher = passwordHasher;
    public readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UpdateUserResponse> Execute(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        await ValidateUserExists(command.Id, cancellationToken);
        await ValidateEmailUniqueness(command.Email, command.Id, cancellationToken);

        var user = await _unitOfWork.Users.GetBy(command.Id, cancellationToken);
        
        UpdateUserBasicInfo(user.Value, command);
        await UpdateUserPassword(user.Value, command);
        await SaveUserChanges(user.Value, cancellationToken);
        await UpdateUserRoles(command, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);
        return new UpdateUserResponse(user.Value.Id);
    }

    private async Task ValidateUserExists(Guid userId, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.Users.GetBy(userId, cancellationToken);
        if (existingUser.HasNoValue)
        {
            throw new NotFoundException(AuthApi_Resource.USER_NOT_EXISTS);
        }
    }

    private async Task ValidateEmailUniqueness(string newEmail, Guid userId, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.Users.GetBy(userId, cancellationToken);
        if (existingUser.Value.Email != newEmail)
        {
            var emailExists = await _unitOfWork.Users.Exists(newEmail, cancellationToken);
            if (emailExists)
            {
                throw new ConflictException(AuthApi_Resource.USER_EXISTS);
            }
        }
    }

    private void UpdateUserBasicInfo(User user, UpdateUserCommand command)
    {
        user.UpdateBasicInfo(command.Name, command.Email);
    }

    private async Task UpdateUserPassword(User user, UpdateUserCommand command)
    {
        if (!string.IsNullOrEmpty(command.Password))
        {
            var newHash = _passwordHasher.Hash(command.Password);
            user.SetHash(newHash);
        }

        await Task.CompletedTask;
    }

    private async Task SaveUserChanges(User user, CancellationToken cancellationToken)
    {
        _unitOfWork.Users.Update(user);
        await Task.CompletedTask;
    }

    private async Task UpdateUserRoles(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        await RemoveExistingUserRoles(command.Id, cancellationToken);
        await AddNewUserRoles(command, cancellationToken);
    }

    private async Task RemoveExistingUserRoles(Guid userId, CancellationToken cancellationToken)
    {
        await _unitOfWork.UserRoles.DeleteByUserId(userId, cancellationToken);
    }

    private async Task AddNewUserRoles(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var roles = await _unitOfWork.Roles.GetBy(command.Roles.Select(s => s.Code), cancellationToken);
        if (!roles.Any() || roles.Count() < command.Roles.Count())
        {
            throw new ValidationException(AuthApi_Resource.INVALID_ROLES, new Dictionary<string, string[]>
            {
                { "Roles", new[] { AuthApi_Resource.INVALID_ROLES } }
            });
        }

        var userRoles = CreateUserRoles(command.Id, roles);
        await _unitOfWork.UserRoles.Insert(userRoles, cancellationToken);
    }

    private List<UserRole> CreateUserRoles(Guid userId, IEnumerable<Role> roles)
    {
        var userRoles = new List<UserRole>();
        foreach (var role in roles)
        {
            userRoles.Add(UserRole.Create(userId, role.Id));
        }

        return userRoles;
    }
}

public record UpdateUserResponse(Guid Id);
