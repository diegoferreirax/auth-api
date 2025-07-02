using AuthApi.Application.Infrastructure.Security.Bcrypt;
using AuthApi.Application.Infrastructure.UnitOfWork;
using AuthApi.Application.Resource;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users.RegisterUser.v1;

public sealed class RegisterUserHandler(
    UserRepository userRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
{
    public readonly UserRepository _userRepository = userRepository;
    public readonly IPasswordHasher _passwordHasher = passwordHasher;
    public readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<RegisterUserResponse>> Execute(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var roles = new List<Role>();
        foreach (var item in command.Roles)
        {
            roles.Add(Role.Create(item.Name).Value);  
        }

        var user = User.Create(command.Name, command.Email, roles);
        if (user.IsFailure)
        {
            return Result.Failure<RegisterUserResponse>(user.Error);
        }

        var userExist = await _userRepository.Exists(command.Email, cancellationToken);
        if (userExist)
        {
            return Result.Failure<RegisterUserResponse>(AuthApi_Resource.USER_EXISTS);
        }

        var hash = _passwordHasher.Hash(command.Password);
        user.Value.SetHash(hash);

        await _userRepository.Insert(user.Value, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success(new RegisterUserResponse(user.Value.Id));
    }
}
