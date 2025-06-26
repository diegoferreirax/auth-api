using AuthApi.Application.Infrastructure.Security.Bcrypt;
using AuthApi.Application.Infrastructure.Security.JWT;
using AuthApi.Application.Resource;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users.AuthenticateUser.v1;

public sealed class AuthenticateUserHandler
{
    public readonly UserRepository _userRepository;
    public readonly TokenService _tokenService;
    public readonly IPasswordHasher _passwordHasher;

    public AuthenticateUserHandler(
        UserRepository userRepository,
        TokenService tokenService,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<AuthenticateUserResponse>> Execute(AuthenticateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Get(command.Email);
        if (user.HasNoValue)
        {
            return Result.Failure<AuthenticateUserResponse>(AuthApi_Resource.INVALID_DATA);
        }

        if (!_passwordHasher.Verify(command.Password, user.Value.Hash))
        {
            return Result.Failure<AuthenticateUserResponse>(AuthApi_Resource.INVALID_DATA);
        }

        var token = _tokenService.GenerateToken(user.Value.Email, user.Value.Roles.FirstOrDefault().Name);

        return Result.Success(new AuthenticateUserResponse(user.Value.Email, token));
    }
}
