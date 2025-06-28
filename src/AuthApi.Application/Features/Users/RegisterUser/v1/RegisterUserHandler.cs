using AuthApi.Application.Infrastructure.Security.Bcrypt;
using AuthApi.Application.Infrastructure.UnitOfWork;
using AuthApi.Application.Resource;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users.RegisterUser.v1;

public sealed class RegisterUserHandler
{
    public readonly UserRepository _userRepository;
    public readonly IPasswordHasher _passwordHasher;
    public readonly IUnitOfWork _unitOfWork;

    public RegisterUserHandler(
        UserRepository userRepository, 
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

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

        var userExist = await _userRepository.Exists(command.Email);
        if (userExist)
        {
            return Result.Failure<RegisterUserResponse>(AuthApi_Resource.USER_EXISTS);
        }

        var hash = _passwordHasher.Hash(command.Password);
        user.Value.SetHash(hash);

        await _userRepository.Insert(user.Value);
        await _unitOfWork.CommitAsync(cancellationToken);

        return Result.Success(new RegisterUserResponse(user.Value.Id));
    }
}
