using AuthApi.Application.Resource;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users.AuthenticateUser.v1;

public class AuthenticateUserCommand
{
    public string Email { get; private set; }
    public string Password { get; private set; }

    private AuthenticateUserCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public static Result<AuthenticateUserCommand> Create(string email, string password)
    {
        if (string.IsNullOrEmpty(email))
        {
            return Result.Failure<AuthenticateUserCommand>(AuthApi_Resource.NAME_REQUIRED);
        }

        if (string.IsNullOrEmpty(password))
        {
            return Result.Failure<AuthenticateUserCommand>(AuthApi_Resource.PASSWORD_REQUIRED);
        }

        return Result.Success(new AuthenticateUserCommand(email, password));
    }
}
