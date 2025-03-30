using AuthApi.Application.Infrastructure.Security.JWT;
using AuthApi.Application.Resource;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users.AuthenticateUser.v1;

public sealed class AuthenticateUserHandler
{
    public readonly UserRepository _userRepository;
    public readonly TokenService _tokenService;

    public AuthenticateUserHandler(
        UserRepository userRepository,
        TokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<Result<AuthenticateUserResponse>> Execute(AuthenticateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Get(command.Name, command.Password);
        if (user.HasNoValue)
        {
            return Result.Failure<AuthenticateUserResponse>(AuthApi_Resource.INVALID_DATA);
        }

        var token = _tokenService.GenerateToken(user.Value.Name, user.Value.Role);

        return Result.Success(new AuthenticateUserResponse(user.Value.Name, token));
    }
}
