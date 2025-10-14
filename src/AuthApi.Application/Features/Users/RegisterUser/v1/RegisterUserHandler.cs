using AuthApi.Application.Persistence.UnitOfWork;
using AuthApi.Application.Resource;
using AuthApi.Application.Security.Bcrypt;
using AuthApi.Application.Exceptions;

namespace AuthApi.Application.Features.Users.RegisterUser.v1;

public sealed class RegisterUserHandler(
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
{
    public readonly IPasswordHasher _passwordHasher = passwordHasher;
    public readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<RegisterUserResponse> Execute(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var user = User.Create(command.Name, command.Email);

        var userExist = await _unitOfWork.Users.Exists(command.Email, cancellationToken);
        if (userExist)
        {
            throw new ConflictException(AuthApi_Resource.USER_EXISTS);
        }

        var hash = _passwordHasher.Hash(command.Password);
        user.SetHash(hash);

        var userAdded = await _unitOfWork.Users.Insert(user, cancellationToken);

        var roles = await _unitOfWork.Roles.GetBy(command.Roles.Select(s => s.Code), cancellationToken);

        var userRoles = new List<UserRole>();
        foreach (var role in roles)
        {
            userRoles.Add(UserRole.Create(user.Id, role.Id));
        }

        await _unitOfWork.UserRoles.Insert(userRoles, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);
        return new RegisterUserResponse(user.Id);
    }
}
