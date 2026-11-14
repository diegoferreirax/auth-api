using AuthApi.Application.Persistence.UnitOfWork;
using AuthApi.Application.Common.Resource;
using AuthApi.Domain.Exceptions;
using AuthApi.Application.Common.Security.Bcrypt;
using AuthApi.Domain.Entities;
using AuthApi.Application.Features.Users.Commands.RegisterUser.v1.Dto;

namespace AuthApi.Application.Features.Users.Commands.RegisterUser.v1;

public sealed class RegisterUserHandler(
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
{
    public readonly IPasswordHasher _passwordHasher = passwordHasher;
    public readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<RegisterUserResponseDto> Execute(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var userExist = await _unitOfWork.Users.Exists(command.Email.Value, cancellationToken);
        if (userExist)
        {
            throw new ConflictException(AuthApi_Resource.USER_EXISTS);
        }

        var roles = await _unitOfWork.Roles.GetBy(command.Roles.Select(s => s.Code.Value), cancellationToken);
        if (!roles.Any() || roles.Count() < command.Roles.Count())
        {
            throw new ValidationException(AuthApi_Resource.INVALID_ROLES, new Dictionary<string, string[]>
            {
                { "Roles", new[] { AuthApi_Resource.INVALID_ROLES } }
            });
        }

        var hash = _passwordHasher.Hash(command.Password.Value);
        var user = User.Create(command.Name.Value, command.Email.Value);
        user.SetHash(hash);

        var userRoles = new List<UserRole>();
        foreach (var role in roles)
        {
            userRoles.Add(UserRole.Create(user.Id, role.Id));
        }

        _unitOfWork.Users.Insert(user);
        _unitOfWork.UserRoles.Insert(userRoles);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new RegisterUserResponseDto(user.Id);
    }
}
