using AuthApi.Application.Resource;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users.RegisterUser.v1;

public sealed class RegisterUserHandler
{
    public readonly UserRepository _userRepository;

    public RegisterUserHandler(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<RegisterUserResponse>> Execute(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var user = User.Create(command.Name, command.Email, command.Password, command.Role);
        if (user.IsFailure)
        {
            return Result.Failure<RegisterUserResponse>(user.Error);
        }

        var userExist = await _userRepository.Exists(command.Name);
        if (userExist)
        {
            return Result.Failure<RegisterUserResponse>(AuthApi_Resource.USER_EXISTS);
        }

        await _userRepository.Insert(user.Value);

        return Result.Success(new RegisterUserResponse(user.Value.Id));
    }
}
