using AuthApi.Application.Infrastructure.Security.Bcrypt;
using AuthApi.Application.Infrastructure.Security.JWT;
using AuthApi.Application.Resource;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users.AuthenticateUser.v1;

public sealed class AuthenticateUserHandler(
    UserRepository userRepository,
    TokenService tokenService,
    IPasswordHasher passwordHasher)
{
    public readonly UserRepository _userRepository = userRepository;
    public readonly TokenService _tokenService = tokenService;
    public readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<Result<AuthenticateUserResponse>> Execute(AuthenticateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetBy(command.Email, cancellationToken);
        if (user.HasNoValue)
        {
            return Result.Failure<AuthenticateUserResponse>(AuthApi_Resource.INVALID_DATA);
        }

        if (!_passwordHasher.Verify(command.Password, user.Value.Hash))
        {
            return Result.Failure<AuthenticateUserResponse>(AuthApi_Resource.INVALID_DATA);
        }

        var token = _tokenService.GenerateToken(user.Value.Email, user.Value.Roles.Select(s => s.Name));

        return Result.Success(new AuthenticateUserResponse(user.Value.Email, token));
    }
}
