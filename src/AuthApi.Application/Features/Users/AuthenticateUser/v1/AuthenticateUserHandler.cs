using AuthApi.Application.Persistence.UnitOfWork;
using AuthApi.Application.Resource;
using AuthApi.Application.Security.Bcrypt;
using AuthApi.Application.Security.JWT;
using AuthApi.Application.Exceptions;

namespace AuthApi.Application.Features.Users.AuthenticateUser.v1;

public sealed class AuthenticateUserHandler(
    TokenService tokenService,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
{
    public readonly TokenService _tokenService = tokenService;
    public readonly IPasswordHasher _passwordHasher = passwordHasher;
    public readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<AuthenticateUserResponse> Execute(AuthenticateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetBy(command.Email, cancellationToken);
        if (user.HasNoValue)
        {
            throw new UnauthorizedException(AuthApi_Resource.INVALID_DATA);
        }

        if (!_passwordHasher.Verify(command.Password, user.Value.Hash))
        {
            throw new UnauthorizedException(AuthApi_Resource.INVALID_DATA);
        }

        var roleIds = user.Value.UserRoles.Select(s => s.IdRole).ToList();
        var userRoles = await _unitOfWork.Roles.GetBy(roleIds, cancellationToken);

        var token = _tokenService.GenerateToken(user.Value.Email, userRoles.Select(s => s.Code));

        return new AuthenticateUserResponse(user.Value.Email, token);
    }
}
