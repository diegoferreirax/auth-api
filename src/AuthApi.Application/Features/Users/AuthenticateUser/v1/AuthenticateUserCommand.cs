using AuthApi.Application.Resource;
using CSharpFunctionalExtensions;

namespace AuthApi.Application.Features.Users.AuthenticateUser.v1;

public class AuthenticateUserCommand
{
    public string Name { get; private set; }
    public string Password { get; private set; }

    private AuthenticateUserCommand(string name, string password)
    {
        Name = name;
        Password = password;
    }

    public static Result<AuthenticateUserCommand> Create(string name, string password)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Result.Failure<AuthenticateUserCommand>(AuthApi_Resource.NAME_REQUIRED);
        }

        if (string.IsNullOrEmpty(password))
        {
            return Result.Failure<AuthenticateUserCommand>(AuthApi_Resource.PASSWORD_REQUIRED);
        }

        return Result.Success(new AuthenticateUserCommand(name, password));
    }
}
