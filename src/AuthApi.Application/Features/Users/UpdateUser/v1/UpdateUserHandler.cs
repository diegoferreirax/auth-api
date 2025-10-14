using AuthApi.Application.Persistence.UnitOfWork;
using AuthApi.Application.Resource;
using AuthApi.Application.Security.Bcrypt;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users.UpdateUser.v1;

public sealed class UpdateUserHandler(
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
{
    public readonly IPasswordHasher _passwordHasher = passwordHasher;
    public readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<UpdateUserResponse>> Execute(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateUserExists(command.Id, cancellationToken);
        if (validationResult.IsFailure)
        {
            return Result.Failure<UpdateUserResponse>(validationResult.Error);
        }

        var emailValidationResult = await ValidateEmailUniqueness(command.Email, validationResult.Value, cancellationToken);
        if (emailValidationResult.IsFailure)
        {
            return Result.Failure<UpdateUserResponse>(emailValidationResult.Error);
        }

        var user = emailValidationResult.Value;
        
        UpdateUserBasicInfo(user, command);
        await UpdateUserPassword(user, command);
        await SaveUserChanges(user, cancellationToken);
        await UpdateUserRoles(command, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);
        return Result.Success(new UpdateUserResponse(user.Id));
    }

    private async Task<Result<User>> ValidateUserExists(Guid userId, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.Users.GetBy(userId, cancellationToken);
        if (existingUser.HasNoValue)
        {
            return Result.Failure<User>(AuthApi_Resource.USER_NOT_EXISTS);
        }

        return Result.Success(existingUser.Value);
    }

    private async Task<Result<User>> ValidateEmailUniqueness(string newEmail, User existingUser, CancellationToken cancellationToken)
    {
        if (existingUser.Email != newEmail)
        {
            var emailExists = await _unitOfWork.Users.Exists(newEmail, cancellationToken);
            if (emailExists)
            {
                return Result.Failure<User>(AuthApi_Resource.USER_EXISTS);
            }
        }

        return Result.Success(existingUser);
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
        _unitOfWork.Users.Update(user, cancellationToken);
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
        var userRoles = CreateUserRoles(command.Id, roles);

        await _unitOfWork.UserRoles.Insert(userRoles, cancellationToken);
    }

    private List<UserRole> CreateUserRoles(Guid userId, IEnumerable<Role> roles)
    {
        var userRoles = new List<UserRole>();
        foreach (var role in roles)
        {
            userRoles.Add(UserRole.Create(userId, role.Id).Value);
        }

        return userRoles;
    }
}

public record UpdateUserResponse(Guid Id);
