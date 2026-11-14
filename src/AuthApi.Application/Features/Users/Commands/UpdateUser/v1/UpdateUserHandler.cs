using AuthApi.Application.Persistence.UnitOfWork;
using AuthApi.Application.Common.Resource;
using AuthApi.Domain.Exceptions;
using AuthApi.Application.Common.Security.Bcrypt;
using AuthApi.Domain.Entities;
using AuthApi.Domain.ValueObjects;

using AuthApi.Application.Features.Users.Commands.UpdateUser.v1.Dto;

namespace AuthApi.Application.Features.Users.Commands.UpdateUser.v1;

public sealed class UpdateUserHandler(
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
{
    public readonly IPasswordHasher _passwordHasher = passwordHasher;
    public readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UpdateUserResponseDto> Execute(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetBy(command.Id, cancellationToken);
        if (user.HasNoValue)
        {
            throw new NotFoundException(AuthApi_Resource.USER_NOT_EXISTS);
        }

        await ValidateEmailUniqueness(command.Email, user.Value, cancellationToken);

        UpdateUserBasicInfo(user.Value, command);
        UpdateUserPassword(user.Value, command);
        SaveUserChanges(user.Value);
        await UpdateUserRoles(command, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);
        return new UpdateUserResponseDto(user.Value.Id);
    }

    private async Task ValidateEmailUniqueness(Email newEmail, User user, CancellationToken cancellationToken)
    {
        if (user.Email != newEmail.Value)
        {
            var emailExists = await _unitOfWork.Users.Exists(newEmail.Value, cancellationToken);
            if (emailExists)
            {
                throw new ConflictException(AuthApi_Resource.USER_EXISTS);
            }
        }
    }

    private static void UpdateUserBasicInfo(User user, UpdateUserCommand command)
    {
        user.UpdateBasicInfo(command.Name.Value, command.Email.Value);
    }

    private void UpdateUserPassword(User user, UpdateUserCommand command)
    {
        if (command.Password is not null)
        {
            var newHash = _passwordHasher.Hash(command.Password.Value);
            user.SetHash(newHash);
        }
    }

    private void SaveUserChanges(User user)
    {
        _unitOfWork.Users.Update(user);
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
        var roles = await _unitOfWork.Roles.GetBy(command.Roles.Select(s => s.Code.Value), cancellationToken);
        if (!roles.Any() || roles.Count() < command.Roles.Count())
        {
            throw new ValidationException(AuthApi_Resource.INVALID_ROLES, new Dictionary<string, string[]>
            {
                { "Roles", new[] { AuthApi_Resource.INVALID_ROLES } }
            });
        }

        var userRoles = CreateUserRoles(command.Id, roles);
        _unitOfWork.UserRoles.Insert(userRoles);
    }

    private static List<UserRole> CreateUserRoles(Guid userId, IEnumerable<Role> roles)
    {
        var userRoles = new List<UserRole>();
        foreach (var role in roles)
        {
            userRoles.Add(UserRole.Create(userId, role.Id));
        }

        return userRoles;
    }
}
