using AuthApi.Application.Common.Resource;
using AuthApi.Application.Exceptions;

namespace AuthApi.Application.Features.Users.Commands.AuthenticateUser.v1;

public class AuthenticateUserCommand
{
    public string Email { get; private set; }
    public string Password { get; private set; }

    private AuthenticateUserCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public static AuthenticateUserCommand Create(string email, string password)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new BusinessException(AuthApi_Resource.EMAIL_REQUIRED);
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new BusinessException(AuthApi_Resource.PASSWORD_REQUIRED);
        }

        return new AuthenticateUserCommand(email, password);
    }
}
